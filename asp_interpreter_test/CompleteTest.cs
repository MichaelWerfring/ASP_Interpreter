//-----------------------------------------------------------------------
// <copyright file="CompleteTest.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_test
{
    using Asp_interpreter_lib.FunctorNaming;
    using Asp_interpreter_lib.InternalProgramClasses.Database;
    using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
    using Asp_interpreter_lib.Preprocessing;
    using Asp_interpreter_lib.Preprocessing.DualRules;
    using Asp_interpreter_lib.Preprocessing.NMRCheck;
    using Asp_interpreter_lib.Preprocessing.OLONDetection;
    using Asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
    using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Util;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    internal class CompleteTest
    {
        [Test]
        public void TestGetProgram()
        {
            var prefixes = new PrefixOptions("fa_", "eh", "chk_", "not_", "V");
            var logger = new TestingLogger(LogLevel.Error);
            string code =
                """
                nat(0).
                nat(s(X)) :- nat(X).

                calculate(X, Y, add, Z) :- nat(X), nat(Y), add(X,Y,Z).
                calculate(X, Y, sub, Z) :- nat(X), nat(Y), sub(X,Y,Z).
                calculate(X, Y, mult, Z) :- nat(X), nat(Y), mult(X,Y,Z).
                calculate(X, Y, div, Z) :- nat(X), nat(Y), div(X,Y,0,Z).

                add(Y, 0, Y).
                add(0, Y, Y).
                add(s(X),Y, s(Z1)) :- add(X, Y, Z1). 

                sub(X,0,X).
                sub(s(X), s(Y), Z) :- sub(X, Y, Z).

                mult(X,s(0),X).
                mult(X,s(N), Z) :- mult(X,N, Z1), add(Z1, X, Z).

                div(0, Y, Acc, Acc).
                div(X, Y, Acc, Z) :- sub(X, Y, Z1), add(Acc, s(0), Acc1),  div(Z1, Y, Acc1, Z).

                ?- calculate(s(s(0)), s(s(0)), mult, X).
                """;

            var program = AspExtensions.GetProgram(code, logger);

            // Dual
            var dualGenerator = new DualRuleConverter(prefixes, logger);
            var dual = dualGenerator.GetDualRules(program.Duplicate().Statements, "_");

            // OLON
            List<Statement> olonRules = new OLONRulesFilterer(logger).FilterOlonRules(program.Statements);

            // NMR
            var nmrChecker = new NmrChecker(prefixes, logger);
            var constraints = nmrChecker.GetConstraintRules(program);
            olonRules.AddRange(constraints);

            var subcheck = nmrChecker.GetNmrCheck(olonRules.Duplicate());

            // Concatenate
            var fullProgram = new AspProgram(
                [.. program.Statements, .. dual, .. subcheck],
                program.Query,
                program.Explanations);

            var solutions = this.GetSolutions(
                fullProgram.Statements,
                fullProgram.Query.GetValueOrThrow().Goals,
                logger);

            Assert.That(logger.ErrorMessages.Count == 0);
            Assert.That(solutions.Count == 1);
            Assert.That(solutions[0].CHSEntries.ToList().ListToString(), Is.EqualTo("add(0, s(s(0)), s(s(0))), " +
                "add(s(0), s(s(0)), s(s(s(0)))), add(s(s(0)), s(s(0)), s(s(s(s(0))))), calculate(s(s(0)), s(s(0)), " +
                "mult, s(s(s(s(0))))), mult(s(s(0)), s(0), s(s(0))), mult(s(s(0)), s(s(0)), s(s(s(s(0)))))," +
                " nat(0), nat(s(0)), nat(s(s(0)))"));
            Assert.That(solutions[0].SolutionMapping.Keys.ToList().ListToString(), Is.EqualTo("X"));
            Assert.That(solutions[0].SolutionMapping.Values.ToList().ListToString(), Is.EqualTo("Term:s(s(s(s(0))))"));
        }

        private List<CoSLDSolution> GetSolutions(List<Statement> rules, List<Goal> query, ILogger logger)
        {
            var converter = new ProgramConverter(new FunctorTableRecord(), logger);
            var convertedRules = rules.Where(rule => rule.Head.HasValue).Select(converter.ConvertStatement);

            var goalConverter = new GoalConverter(new FunctorTableRecord());
            var convertedQuery = query.Select(x => goalConverter.Convert(x).GetValueOrThrow());

            var database = new DualClauseDatabase(convertedRules, new FunctorTableRecord());

            var solver = new CoinductiveSLDSolver(database, new FunctorTableRecord(), logger);

            var result = new List<CoSLDSolution>();
            foreach (var solution in solver.Solve(convertedQuery.Append(new Structure("_nmr_check",[]))))
            {
                result.Add(solution);
            }

            return result;
        }
    }
}
﻿//-----------------------------------------------------------------------
// <copyright file="Application.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_exe
{
    using Antlr4.Runtime;
    using Asp_interpreter_lib.FunctorNaming;
    using Asp_interpreter_lib.InternalProgramClasses.Database;
    using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
    using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
    using Asp_interpreter_lib.Preprocessing;
    using Asp_interpreter_lib.Preprocessing.DualRules;
    using Asp_interpreter_lib.Preprocessing.NMRCheck;
    using Asp_interpreter_lib.Preprocessing.OLONDetection;
    using Asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
    using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;
    using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;
    using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
    using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
    using Asp_interpreter_lib.Unification.Constructive.Unification.Standard;
    using Asp_interpreter_lib.Util;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using Asp_interpreter_lib.Util.ErrorHandling.Either;
    using Asp_interpreter_lib.Visitors;
    using System.IO.IsolatedStorage;

    /// <summary>
    /// Represents the console application for the ASP interpreter.
    /// </summary>
    public class Application
    {
        private readonly ILogger logger;

        private readonly ProgramVisitor programVisitor;

        private readonly ProgramConfig config;

        private readonly PrefixOptions prefixes = new("fa_", "eh", "chk_", "not_", "V");

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="logger">The logger to display messages to the user interface.</param>
        /// <param name="programVisitor">Visitor to traverse the parse tree and retrieve the internal representation of the asp program.</param>
        /// <param name="config">Configuration options for running the interpreter.</param>
        /// <exception cref="ArgumentNullException">Is thrown if one of the arguments is null.</exception>
        public Application(ILogger logger, ProgramVisitor programVisitor, ProgramConfig config)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.programVisitor = programVisitor ?? throw new ArgumentNullException(nameof(programVisitor));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Runs the application.
        /// </summary>
        public void Run()
        {
            if (this.config.DisplayExplanation)
            {
                this.ExplainProgram();
                return;
            }

            var eitherProgram = this.LoadProgram();

            if (!eitherProgram.IsRight)
            {
                this.logger.LogError(eitherProgram.GetLeftOrThrow());
                return;
            }

            var program = eitherProgram.GetRightOrThrow();

            // Interactive if needed else just solve
            if (!this.config.RunInteractive)
            {
                if (!program.Query.HasValue)
                {
                    this.logger.LogError("The program has no query given, either specify one in the file or use interactive mode.");
                    return;
                }

                this.SolveAutomatic(program);
                return;
            }

            // If a query is specified in the file it will be ignored in this case
            while (true)
            {
                Console.Write("?-");
                string input = Console.ReadLine() ?? string.Empty;

                if (input == "exit")
                {
                    return;
                }

                if (input == "clear")
                {
                    Console.Clear();
                    continue;
                }

                if (input == "reload")
                {
                    var either = this.LoadProgram();
                    if (!either.IsRight)
                    {
                        this.logger.LogError(either.GetLeftOrThrow());
                        return;
                    }

                    program = either.GetRightOrThrow();
                    continue;
                }

                // 1) parse query
                var query = this.ParseQuery("?-" + input);
                if (query == null)
                {
                    continue;
                }

                // 2) solve existing program with new query
                // 3) show answer
                this.InteractiveSolve(program.Statements, program.LiteralsToShow, query.Goals);
            }
        }

        private static void PrintSolution(CoSLDSolution solution)
        {
            Console.WriteLine("Solution found!");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Mapping:");
            PrintMapping(solution.SolutionMapping);
            Console.WriteLine("CHS:");
            Console.WriteLine($"{{ {solution.CHSEntries.ToList().ListToString()} }}");
            Console.WriteLine("---------------------------------------------------------------");
        }

        private static void PrintMapping(VariableMapping mapping)
        {
            foreach (var pair in mapping)
            {
                if (pair.Value is TermBinding termBinding)
                {
                    Console.WriteLine($"{pair.Key} = {termBinding.Term}");
                }
                else if (pair.Value is ProhibitedValuesBinding binding)
                {
                    Console.WriteLine($"{pair.Key} \\= {{ {binding.ProhibitedValues.ToList().ListToString()} }}");
                }
            }
        }

        private void ExplainProgram()
        {
            var code = FileReader.ReadFile(this.config.FilePath);

            if (!code.IsRight)
            {
                this.logger.LogError(code.GetLeftOrThrow());
                return;
            }

            var program = this.GetProgram(code.GetRightOrThrow());
            var visitor = new ExplainProgramVisitor(program, this.logger);

            foreach (var statement in program.Statements)
            {
                statement.Accept(visitor).IfHasValue(Console.WriteLine);
            }
        }

        private IEither<string, AspProgram> LoadProgram()
        {
            if (string.IsNullOrEmpty(this.config.FilePath))
            {
                return new Left<string, AspProgram>("No file path specified!");
            }

            // Read
            var code = FileReader.ReadFile(this.config.FilePath);

            if (!code.IsRight)
            {
                return new Left<string, AspProgram>(code.GetLeftOrThrow());
            }

            // Program
            var program = this.GetProgram(code.GetRightOrThrow());

            // Dual
            var dualGenerator = new DualRuleConverter(this.prefixes, this.logger);
            var dual = dualGenerator.GetDualRules(program.Duplicate().Statements, "_");

            // OLON
            List<Statement> olonRules = new OLONRulesFilterer(this.logger).FilterOlonRules(program.Statements);

            // NMR
            var nmrChecker = new NmrChecker(this.prefixes, this.logger);
            var constraints = nmrChecker.GetConstraintRules(program);
            olonRules.AddRange(constraints);

            var subcheck = nmrChecker.GetNmrCheck(olonRules.Duplicate());

            return new Right<string, AspProgram>(new AspProgram(
                [.. program.Statements, .. dual, .. subcheck],
                program.Query,
                program.Explanations,
                program.LiteralsToShow));
        }

        private Query? ParseQuery(string query)
        {
            var inputStream = new AntlrInputStream(query);
            var lexer = new ASPLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new ASPParser(commonTokenStream);
            var context = parser.query();
            var visitor = new QueryVisitor(this.logger);
            var parsedQuery = visitor.VisitQuery(context);

            if (!parsedQuery.HasValue)
            {
                this.logger.LogError("Not able to parse query: " + query);
                return null;
            }

            return parsedQuery.GetValueOrThrow();
        }

        private void SolveAutomatic(AspProgram program)
        {
            var functors = new FunctorTableRecord();

            var converter = new ProgramConverter(functors, this.logger);

            var convertedStatements = program.Statements.Where(x => x.HasHead).Select(converter.ConvertStatement).ToList();

            var convertedQuery = converter.ConvertQuery(program.Query.GetValueOrThrow());

            var database = new DualClauseDatabase(convertedStatements, functors);

            var goalConverterForLiteralsToKeep = new GoalConverter(functors);
            var predicatesToKeep = program.LiteralsToShow.Select(x => goalConverterForLiteralsToKeep.Convert(x).GetValueOrThrow()).ToList();

            var postProcessor = new SolutionPostprocessor(
                new VariableMappingPostprocessor(),
                new CHSPostProcessor(
                    functors,
                    predicatesToKeep,
                    new StandardConstructiveUnificationAlgorithm(false)));

            var solver = new CoinductiveSLDSolver(database, functors, postProcessor, this.logger);

            var appendedQuery = convertedQuery.Append(new Structure("_nmr_check", new List<ISimpleTerm>()));

            foreach (var solution in solver.Solve(appendedQuery))
            {
                PrintSolution(solution);
            }
        }

        private void InteractiveSolve(List<Statement> rules, List<Literal> literalsToShow, List<Goal> query)
        {
            var functors = new FunctorTableRecord();

            var converter = new ProgramConverter(functors, this.logger);
            var convertedRules = rules.Where(rule => rule.Head.HasValue).Select(converter.ConvertStatement);

            var goalConverter = new GoalConverter(functors);
            var convertedQuery = query.Select(x => goalConverter.Convert(x).GetValueOrThrow());

            var database = new DualClauseDatabase(convertedRules, functors);

            var goalConverterForLiteralsToKeep = new GoalConverter(functors);
            var predicatesToKeep = literalsToShow.Select(x => goalConverterForLiteralsToKeep.Convert(x).GetValueOrThrow()).ToList();

            var postProcessor = new SolutionPostprocessor(
                new VariableMappingPostprocessor(),
                new CHSPostProcessor(
                functors,
                predicatesToKeep,
                new StandardConstructiveUnificationAlgorithm(false)));

            var solver = new CoinductiveSLDSolver(database, functors, postProcessor, this.logger);

            foreach (var solution in solver.Solve(convertedQuery.Append(new Structure("_nmr_check", new List<ISimpleTerm>()))))
            {
                PrintSolution(solution);
            }
        }

        private AspProgram GetProgram(string code)
        {
            var inputStream = new AntlrInputStream(code);
            var lexer = new ASPLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new ASPParser(commonTokenStream);
            var context = parser.program();
            var program = this.programVisitor.VisitProgram(context);

            if (!program.HasValue)
            {
                throw new ArgumentException("Failed to parse program!");
            }

            return program.GetValueOrThrow();
        }
    }
}
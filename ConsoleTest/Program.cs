using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Preprocessing.OLONDetection;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Solving.NMRCheck;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Unification.Basic.Robinson;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;
using System.Diagnostics;

namespace ConsoleTest;

internal class Program
{
    static void Main(string[] args)
    {
        var logger = new ConsoleLogger(LogLevel.None);

        var program = ConstructFullProgram(args[0], logger);

        var converter = new ProgramConverter(new FunctorTableRecord(), logger);
        var internalProgram = converter.Convert(program);

        var database = new DualClauseDatabase(internalProgram.Statements, new FunctorTableRecord());

        var appendedQuery = internalProgram.Query.Concat([new Structure("nmr_check", [])]);

        PrintProgram(program, internalProgram.Statements, appendedQuery);

        var sw = new Stopwatch();

        Console.WriteLine(sw.ElapsedMilliseconds.ToString());

        var solver1 = new CoinductiveSLDSolver(database, new FunctorTableRecord());

        sw.Restart();
        var solutions = solver1.Solve(appendedQuery);
        Console.WriteLine("Solutions:");
        foreach (var solution in solutions)
        {
            RenderSolution(solution);
        }
        sw.Stop();

        Console.WriteLine(sw.ElapsedMilliseconds.ToString());

    }
    static AspProgram ConstructFullProgram(string fileString, ILogger logger)
    {
        var fileReadingResult = FileReader.ReadFile(fileString, logger);

        var program = AspExtensions.GetProgram(fileReadingResult!, logger);
        var copy = AspExtensions.GetProgram(fileReadingResult!, logger);

        var dualConverter = new DualRuleConverter(AspExtensions.CommonPrefixes, logger, false);
        var duals = dualConverter.GetDualRules(copy.Statements, false);

        var detector = new OLONRulesFilterer(logger);
        var olonRules = detector.FilterOlonRules(program.Statements);

        var nmrChecker = new NmrChecker(AspExtensions.CommonPrefixes, logger);
        var subcheckRules = nmrChecker.GetSubCheckRules(olonRules, false);

        var newStatements = program.Statements.Concat(duals).Concat(subcheckRules).ToList();
        if (!newStatements.Any(x => x.Head.HasValue && x.Head.GetValueOrThrow().Identifier == "nmr_check"))
        {
            var nmrCheckStatement = new Statement();
            nmrCheckStatement.AddHead(new Literal("nmr_check", false, false, new List<ITerm>()));

            newStatements.Add(nmrCheckStatement);
        }

        return new AspProgram(newStatements, program.Query);
    }

    static void RenderSolution(CoSLDSolution solution)
    {
        Console.WriteLine("Solution found!");
        Console.WriteLine("---------------------------------------------------------------");
        Console.WriteLine("Mapping:");
        RenderMapping(solution.SolutionMapping);
        Console.WriteLine("CHS:");
        RenderCHS(solution.SolutionSet);
        Console.WriteLine("---------------------------------------------------------------");
    }

    static void RenderCHS(CoinductiveHypothesisSet set)
    {
        Console.WriteLine($"{{ {set.Terms.ToList().ListToString()} }}");
    }

    static void RenderMapping(VariableMapping mapping)
    {
        var postProcessor = new VariableMappingPostprocessor();
        var simplifiedMapping = postProcessor.Postprocess(mapping);

        foreach (var pair in simplifiedMapping.Mapping)
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

    static void PrintProgram(AspProgram program, IEnumerable<IEnumerable<ISimpleTerm>> database, IEnumerable<ISimpleTerm> query)
    {
        Console.WriteLine("ASP program:");
        Console.WriteLine(program.ToString());

        Console.WriteLine("Converted program:");

        Console.WriteLine("Statements:");
        Console.WriteLine("----------------------------------------------------------------------------------");
        foreach (var clause in database)
        {
            Console.WriteLine(clause.ToList().ListToString());
        }
        Console.WriteLine("----------------------------------------------------------------------------------");

        Console.WriteLine("Query:");
        Console.WriteLine("----------------------------------------------------------------------------------");
        Console.WriteLine(query.ToList().ListToString());
        Console.WriteLine("----------------------------------------------------------------------------------");
    }

}

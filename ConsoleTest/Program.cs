using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace ConsoleTest;

internal class Program
{
    static void Main(string[] args)
    {
        var logger = new ConsoleLogger(LogLevel.Debug);

        var fileReadingResult = FileReader.ReadFile(args[0], logger);

        var program = AspExtensions.GetProgram(fileReadingResult!, logger);

        //var dualRuleConv = new DualRuleConverter(AspExtensions.CommonPrefixes, logger);
        //var duals = dualRuleConv.GetDualRules(program.Statements);

        //var programWithDuals = new AspProgram(program.Statements.Concat(duals).ToList(), program.Query);

        var converter = new ProgramConverter(new FunctorTableRecord(), logger);

        var internalProgram = converter.Convert(program);

        var database = new StandardDatabase(internalProgram.Statements);

        var solver = new CoinductiveSLDSolver(database, new FunctorTableRecord());

        foreach ( var solution in solver.Solve(internalProgram.Query)) { RenderSolution(solution); }
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
}

using Antlr4.Runtime;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.OLONDetection;
using asp_interpreter_lib.OLONDetection.CallGraph;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Solving.NMRCheck;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Visitors;
using Microsoft.Extensions.Logging;
using QuikGraph;

namespace asp_interpreter_lib.Util;

public class ApplicationOptions
{

}

public class Application(
    ILogger<Application> logger,
    ProgramVisitor programVisitor)
{
    private ILogger<Application> _logger = logger;
    
    private readonly ProgramVisitor _programVisitor = programVisitor;

    public void Run()
    {
        
    }

    private AspProgram GetProgram(string code)
    {
        var inputStream = new AntlrInputStream(code);
        var lexer = new ASPLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new ASPParser(commonTokenStream);
        var context = parser.program();
        var program = _programVisitor.VisitProgram(context);

        if (!program.HasValue)
        {
            throw new ArgumentException("Failed to parse program!");
        }

        return program.GetValueOrThrow();
    } 
    
    private void Show(AspProgram program, string code)
    {
        Console.WriteLine("Program:");
        Console.WriteLine("---------------------------------------------------------------------------");
        Console.WriteLine(program.ToString());
        Console.WriteLine("---------------------------------------------------------------------------");

        Console.WriteLine("Press any key to display dual program...");
        Console.WriteLine("\n\n\n");
        Console.ReadKey();

        Console.WriteLine("Duals:");
        Console.WriteLine("---------------------------------------------------------------------------");
        var prefixes = new PrefixOptions("rwh", "fa", "eh", "chk", "dis", "var");
        DualRuleConverter dualConverter = new(
            prefixes,
            true);
        var duals = dualConverter.GetDualRules(CopyProgram(code).Statements);
        foreach (var dual in duals)
        {
            Console.WriteLine(dual);
        }

        Console.WriteLine("---------------------------------------------------------------------------");

        Console.WriteLine("Press any key to show OLON rules...");
        Console.WriteLine("\n\n\n");
        Console.ReadKey();

        Console.WriteLine("OLON Rules:");
        Console.WriteLine("---------------------------------------------------------------------------");
        //var graphBuilder = new CallGraphBuilder();
        //var callGraph = graphBuilder.BuildCallGraph(program.Statements);
        //PrintAll(program, callGraph);
        List<Statement> olonRules = new OLONRulesFilterer().FilterOlonRules(program.Statements);
        foreach (var rule in olonRules)
        {
            Console.WriteLine(rule.ToString());
        }

        Console.WriteLine("---------------------------------------------------------------------------");

        Console.WriteLine("Press any key to show NMR-Check...");
        Console.WriteLine("\n\n\n");
        Console.ReadKey();

        Console.WriteLine("NMR- Check:");
        Console.WriteLine("---------------------------------------------------------------------------");
        NmrChecker checker = new(prefixes);
        var nmrCheck = checker.GetSubCheckRules(olonRules);

        foreach (var rule in nmrCheck)
        {
            Console.WriteLine(rule.ToString());
        }

        Console.WriteLine("---------------------------------------------------------------------------");
        
        var record = new FunctorTableRecord();
        var converter = new ProgramConverter(record);

        var convertedProgram = converter.Convert(program);
        Console.WriteLine(convertedProgram.ToString());

        var db = new StandardDatabase(convertedProgram.Statements);

        var solver = new AdvancedSLDSolver(db, record);
        solver.SolutionFound += (_, sol) =>
        {
            Console.WriteLine("Solution found!");
            Console.WriteLine("----------------------------------------");
            foreach (var pair in sol.Mapping)
            {
                Console.WriteLine($"{pair.Key} = {pair.Value}");
            }
            Console.WriteLine("----------------------------------------");
        };
        solver.Solve(convertedProgram.Query);
    }

    private void PrintAll(AspProgram program, AdjacencyGraph<Statement, CallGraphEdge?> callGraph)
    {
        Console.WriteLine("Program:");
        Console.WriteLine("---------------------------------------------------------------------------");
        Console.WriteLine(program.ToString());

        Console.WriteLine("Graph:");
        Console.WriteLine("---------------------------------------------------------------------------");
        foreach (var edge in callGraph.Edges)
        {
            Console.WriteLine(edge.ToString());
        }

        Console.WriteLine("---------------------------------------------------------------------------");

        Console.WriteLine("Cycles:");
        Console.WriteLine("---------------------------------------------------------------------------");

        var cycleFinder = new CallGraphCycleFinder();
        var vertexToCycleMapping = cycleFinder.FindAllCycles(callGraph);

        foreach (var vertex in vertexToCycleMapping.Keys)
        {
            var cycles = vertexToCycleMapping[vertex];

            Console.WriteLine($"Cycles of {vertex.ToString()}:");
            Console.WriteLine("---------------------------------------------------------------------------");
            foreach (var cycle in cycles)
            {
                Console.WriteLine(CycleStringifier.CycleToString(cycle));
            }

            Console.WriteLine("---------------------------------------------------------------------------");
        }

        var olonRulesFilterer = new OLONRulesFilterer();
        var olonRules = olonRulesFilterer.FilterOlonRules(program.Statements);

        Console.WriteLine("OLON rules:");
        Console.WriteLine("----------------------------------------------------------------------------");

        foreach (var rule in olonRules)
        {
            Console.WriteLine(rule);
        }
    }

    private AspProgram CopyProgram(string program)
    {
        return AspExtensions.GetProgram(program, new ConsoleErrorLogger());
    }
}
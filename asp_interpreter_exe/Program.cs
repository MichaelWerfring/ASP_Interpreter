using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Antlr4.Runtime;
using asp_interpreter_lib;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.FileIO;
using asp_interpreter_lib.Visitors;
using QuikGraph;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.OLONDetection.CallGraph;
using asp_interpreter_lib.OLONDetection;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Mapping;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.NMRCheck;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Types.TypeVisitors.Copy;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<IErrorLogger, ConsoleErrorLogger>();
builder.Services.AddTransient<ProgramVisitor>();
using var host = builder.Build();

if(args.Length != 1)
{
    throw new ArgumentException("Please provide a source file!");
}

var result = FileReader.ReadFile(args[0]);

if (!result.Success)
{
    throw new ArgumentException(result.Message);
}

var inputStream = new AntlrInputStream(result.Content);
var lexer = new ASPLexer(inputStream);
var commonTokenStream = new CommonTokenStream(lexer);
var parser = new ASPParser(commonTokenStream);
var context = parser.program();
var visitor = host.Services.GetRequiredService<ProgramVisitor>();
var program = visitor.VisitProgram(context);

if (!program.HasValue)
{
    throw new ArgumentException("Failed to parse program!");
}

var prog = program.GetValueOrThrow();

Show(prog, result.Content);
Console.ReadKey();


static void Show(AspProgram program, string code)
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
    var prefixes = new PrefixOptions("rwh", "fa", "eh", "chk", "dis");
    DualRuleConverter dualConverter = new(program,
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
}

static AspProgram CopyProgram(string program)
{
    return ASPExtensions.GetProgram(program, new ConsoleErrorLogger());
}

static void PrintAll(AspProgram program, AdjacencyGraph<Statement, CallGraphEdge?> callGraph)
{
    Console.WriteLine("Program:");
    Console.WriteLine("---------------------------------------------------------------------------");
    Console.WriteLine(program.ToString());

    Console.WriteLine("Graph:");
    Console.WriteLine("---------------------------------------------------------------------------");
    foreach(var edge in callGraph.Edges)
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


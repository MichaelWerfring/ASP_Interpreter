using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Antlr4.Runtime;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.FileIO;
using asp_interpreter_lib.Visitors;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Graphviz;
using System.IO;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.ListExtensions;
using asp_interpreter_lib.OLONDetection.CallGraph;
using asp_interpreter_lib.OLONDetection;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Types.Terms;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<IErrorLogger, ConsoleErrorLogger>();
builder.Services.AddTransient<ProgramVisitor>();
using var host = builder.Build();


//if(args.Length != 1)
//{
//    throw new ArgumentException("Please provide a source file!");
//}

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

var duals = DualRuleConverter.GetDualRules(program.GetValueOrThrow().Statements);

var graphBuilder = new CallGraphBuilder();
var callGraph = graphBuilder.BuildCallGraph(program.GetValueOrThrow().Statements);
PrintAll(program.GetValueOrThrow(), callGraph);

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


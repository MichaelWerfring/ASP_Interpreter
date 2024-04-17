using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Antlr4.Runtime;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.FileIO;
using asp_interpreter_lib.Visitors;
using QuikGraph;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.OLONDetection.CallGraph;
using asp_interpreter_lib.OLONDetection;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.SLDSolverClasses.StandardSolver;
using asp_interpreter_lib.Unification.Robinson;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram;
using asp_interpreter_lib.SLDSolverClasses.VariableRenaming;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.GoalMapping;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals.ArithmeticEvaluation;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals;

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

var prog = program.GetValueOrThrow();
//DualRuleConverter dualConverter = new(prog);
//var duals = dualConverter.GetDualRules(program.GetValueOrThrow().Statements);

var graphBuilder = new CallGraphBuilder();
var callGraph = graphBuilder.BuildCallGraph(program.GetValueOrThrow().Statements);
PrintAll(program.GetValueOrThrow(), callGraph);

//////////////////////////////////////////////////////////////////////////////////////////////////////
Console.WriteLine("---------------------------------------------------------------------------");

var converter = new ProgramConverter();
InternalAspProgram internalProgram = converter.Convert(prog);

IDatabase database = new StandardDatabase(internalProgram.Statements);


var mapping = new Dictionary<(string, int), IGoal>()
{
    {("is", 2), new ArithmeticEvaluationGoal() },
    {("=",2), new UnificationGoal(new RobinsonUnificationAlgorithm(false)) },
    {("\\=",2), new DisunificationGoal(new RobinsonUnificationAlgorithm(false)) },
};

var goalMapper = new GoalMapper(mapping);

var resolver = new GoalResolver(goalMapper);

var nafGoal = new NafGoal(goalMapper);

var solver = new AdvancedSLDSolver(database, resolver);
solver.SolutionFound += (sender, e) =>
{
    Console.WriteLine("---------------------------------------------------------------------------");
    Console.WriteLine("Solution found!");
    foreach(var pair in e.Mapping)
    {
        Console.WriteLine($"{pair.Key} = {pair.Value}");
    }

    Console.WriteLine("---------------------------------------------------------------------------");
};

solver.Solve(internalProgram.Query);

//////////////////////////////////////////////////////////////////////////////////////////////////////

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


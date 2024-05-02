using Antlr4.Runtime;
using asp_interpreter_exe;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.Preprocessing.OLONDetection;
using asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Solving.NMRCheck;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Visitors;
using QuikGraph;
using QuikGraph.Algorithms;

namespace asp_interpreter_lib.Util;

public class Application(
    ILogger logger,
    ProgramVisitor programVisitor, 
    ProgramConfig config)
{
    private readonly ILogger _logger = logger;

    private readonly ProgramVisitor _programVisitor = programVisitor;

    private readonly ProgramConfig config = config;

    private readonly PrefixOptions prefixes = new("rwh", "fa", "eh", "chk", "dis", "var");

    public void Run()
    {
        //Read
        var code = FileReader.ReadFile(config.Path, _logger);
        if (code == null) return;

        //Program
        var program = GetProgram(code);

        //Dual
        var dualGenerator = new DualRuleConverter(prefixes, _logger);
        var dual = dualGenerator.GetDualRules(program.Statements);

        //OLON
        List<Statement> olonRules = new OLONRulesFilterer(_logger).FilterOlonRules(program.Statements);

        //NMR 
        var nmrChecker = new NmrChecker(prefixes, _logger);
        var subcheck = nmrChecker.GetSubCheckRules(olonRules);

        //Interactive if needed else just solve
        if (!config.Interactive)
        {
            if (!program.Query.HasValue)
            {
                _logger.LogError("The program has no query given, either specify one in the file or use interactive mode.");
            }

            //Solve and show answer...
            return;
        }

        //If a query is specified in the file it will be ignored in this case
        while (true)
        {
            Console.Write("?-");
            string input = Console.ReadLine();

            if (input == "exit") return;

            // 1) parse query 
            // 2) solve existing program with new query 
            // 3) show answer
        }
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
            new ConsoleLogger(ErrorHandling.LogLevel.None));
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
        List<Statement> olonRules = new OLONRulesFilterer(_logger).FilterOlonRules(program.Statements);
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
        NmrChecker checker = new(prefixes, new ConsoleLogger(ErrorHandling.LogLevel.None));
        var nmrCheck = checker.GetSubCheckRules(olonRules);

        foreach (var rule in nmrCheck)
        {
            Console.WriteLine(rule.ToString());
        }

        Console.WriteLine("---------------------------------------------------------------------------");
        
        var record = new FunctorTableRecord();
        var converter = new ProgramConverter(record, _logger);

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

        var cycleFinder = new CallGraphCycleFinder(_logger);
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

        var olonRulesFilterer = new OLONRulesFilterer(_logger);
        var olonRules = olonRulesFilterer.FilterOlonRules(program.Statements);

        Console.WriteLine("OLON rules:");
        Console.WriteLine("----------------------------------------------------------------------------");

        foreach (var rule in olonRules)
        {
            Console.WriteLine(rule);
        }
    }

    private static AspProgram CopyProgram(string program)
    {
        return AspExtensions.GetProgram(program, new ConsoleLogger(ErrorHandling.LogLevel.Error));
    }
}
using Antlr4.Runtime;
using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.Database;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.Preprocessing.OLONDetection;
using Asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.Util.ErrorHandling.Either;
using Asp_interpreter_lib.Visitors;
using Asp_interpreter_lib.Preprocessing;
using System.Diagnostics;
using Asp_interpreter_lib.Preprocessing.NMRCheck;
using Asp_interpreter_lib.Preprocessing.DualRules;

namespace Asp_interpreter_exe;

public class Application(
    ILogger logger,
    ProgramVisitor programVisitor,
    ProgramConfig config)
{
    private readonly ILogger _logger = logger;

    private readonly ProgramVisitor _programVisitor = programVisitor;

    private readonly ProgramConfig _config = config;

    private readonly PrefixOptions _prefixes = new("fa_", "eh", "chk_", "not_", "V");

    public void Run()
    {
        if (_config.DisplayExplanation)
        {
            ExplainProgram();
            return;
        }

        var eitherProgram = LoadProgram();

        if (!eitherProgram.IsRight)
        {
            _logger.LogError(eitherProgram.GetLeftOrThrow());
            return;
        }

        var program = eitherProgram.GetRightOrThrow();

        //Interactive if needed else just solve
        if (!_config.RunInteractive)
        {
            if (!program.Query.HasValue)
            {
                _logger.LogError("The program has no query given, either specify one in the file or use interactive mode.");
                return;
            }

            SolveAutomatic(program);
            return;
        }

        //If a query is specified in the file it will be ignored in this case
        while (true)
        {
            Console.Write("?-");
            string input = Console.ReadLine() ?? "";

            if (input == "exit") return;
            if (input == "clear")
            {
                Console.Clear();
                continue;
            }
            if (input == "reload")
            {
                var either = LoadProgram();
                if (!either.IsRight)
                {
                    _logger.LogError(either.GetLeftOrThrow());
                    return;
                }

                program = either.GetRightOrThrow();
                continue;
            }

            // 1) parse query 
            var query = ParseQuery("?-" + input);
            if (query == null) continue;

            // 2) solve existing program with new query 
            // 3) show answer
            InteractiveSolve(program.Statements, query.Goals);
        }
    }

    private void ExplainProgram()
    {
        var code = FileReader.ReadFile(_config.FilePath);

        if (!code.IsRight)
        {
            _logger.LogError(code.GetLeftOrThrow());
            return;
        }

        var program = GetProgram(code.GetRightOrThrow());
        var visitor = new ExplainProgramVisitor(program, logger);

        foreach (var statement in program.Statements)
        {
            statement.Accept(visitor).IfHasValue(v => Console.WriteLine(v));
        }
    }

    private IEither<string, AspProgram> LoadProgram()
    {
        // Read
        var code = FileReader.ReadFile(_config.FilePath);

        if (!code.IsRight)
        {
            return new Left<string, AspProgram>(code.GetLeftOrThrow());
        }

        // Program
        var program = GetProgram(code.GetRightOrThrow());

        // Dual
        var dualGenerator = new DualRuleConverter(_prefixes, _logger);
        var dual = dualGenerator.GetDualRules(program.Duplicate().Statements, "_");

        // OLON
        List<Statement> olonRules = new OLONRulesFilterer(_logger).FilterOlonRules(program.Statements);

        // NMR 
        var nmrChecker = new NmrChecker(_prefixes, _logger);
        var constraints = nmrChecker.GetConstraintRules(program);
        olonRules.AddRange(constraints);

        var subcheck = nmrChecker.GetNmrCheck(olonRules.Duplicate());

        return new Right<string, AspProgram>(new AspProgram(
            [.. program.Statements, .. dual, .. subcheck]
            , program.Query
            , program.Explanations));
    }

    private Query? ParseQuery(string query)
    {
        var inputStream = new AntlrInputStream(query);
        var lexer = new ASPLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new ASPParser(commonTokenStream);
        var context = parser.query();
        var visitor = new QueryVisitor(_logger);
        var parsedQuery = visitor.VisitQuery(context);

        if (!parsedQuery.HasValue)
        {
            _logger.LogError("Not able to parse query: " + query);
            return null;
        }

        return parsedQuery.GetValueOrThrow();
    }

    private void SolveAutomatic(AspProgram program)
    {
        var converter = new ProgramConverter(new FunctorTableRecord(), _logger);

        var convertedStatements = program.Statements.Where(x => x.HasHead).Select(converter.ConvertStatement).ToList();

        var convertedQuery = converter.ConvertQuery(program.Query.GetValueOrThrow());

        var database = new DualClauseDatabase(convertedStatements, new FunctorTableRecord());

        var solver = new CoinductiveSLDSolver(database, new FunctorTableRecord(), _logger);

        var appendedQuery = convertedQuery.Append(new Structure("_nmr_check", []));

        var sw = new Stopwatch();
        sw.Start();
        foreach (var solution in solver.Solve(appendedQuery))
        {
            Console.WriteLine(sw.Elapsed.ToString());
            PrintSolution(solution);
        }
    }

    private void InteractiveSolve(List<Statement> rules, List<Goal> query)
    {
        var converter = new ProgramConverter(new FunctorTableRecord(), _logger);
        var convertedRules = rules.Where(rule => rule.Head.HasValue).Select(converter.ConvertStatement);

        var goalConverter = new GoalConverter(new FunctorTableRecord());
        var convertedQuery = query.Select(x => goalConverter.Convert(x).GetValueOrThrow());

        var database = new DualClauseDatabase(convertedRules, new FunctorTableRecord());

        var solver = new CoinductiveSLDSolver(database, new FunctorTableRecord(), _logger);

        foreach (var solution in solver.Solve(convertedQuery.Append(new Structure("_nmr_check", []))))
        {
            PrintSolution(solution);
        }
    }

    static void PrintSolution(CoSLDSolution solution)
    {
        Console.WriteLine("Solution found!");
        Console.WriteLine("---------------------------------------------------------------");
        Console.WriteLine("Mapping:");
        PrintMapping(solution.SolutionMapping);
        Console.WriteLine("CHS:");
        Console.WriteLine($"{{ {solution.CHSEntries.ToList().ListToString()} }}");
        Console.WriteLine("---------------------------------------------------------------");
    }

    static void PrintMapping(VariableMapping mapping)
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
}

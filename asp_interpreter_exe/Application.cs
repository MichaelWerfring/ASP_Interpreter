﻿using Antlr4.Runtime;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.Preprocessing.OLONDetection;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Solving.NMRCheck;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Util.ErrorHandling.Either;
using asp_interpreter_lib.Visitors;
using System.Net.Http.Headers;

namespace asp_interpreter_lib.Util;

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
        var eitherProgram = LoadProgram();

        if (!eitherProgram.IsRight)
        {
            _logger.LogError(eitherProgram.GetLeftOrThrow());
            return;
        }

        var program = eitherProgram.GetRightOrThrow();

        //Interactive if needed else just solve
        if (!_config.Interactive)
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
                var either= LoadProgram();
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

    private IEither<string, AspProgram> LoadProgram()
    {
        //Read
        var code = FileReader.ReadFile(_config.Path);

        if (!code.IsRight) 
        {
            return new Left<string, AspProgram>(code.GetLeftOrThrow());
        }

        //Program
        var program = GetProgram(code.GetRightOrThrow());

        //Dual
        var dualGenerator = new DualRuleConverter(_prefixes, _logger);
        var dual = dualGenerator.GetDualRules(program.Duplicate().Statements);

        //OLON
        List<Statement> olonRules = new OLONRulesFilterer(_logger).FilterOlonRules(program.Statements);

        //NMR 
        var nmrChecker = new NmrChecker(_prefixes, _logger);
        var subcheck = nmrChecker.GetSubCheckRules(olonRules.Duplicate());

        return new Right<string, AspProgram>(new AspProgram([.. program.Statements, .. dual, .. subcheck], program.Query));
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

        var convertedProgram = converter.Convert(program);
        var database = new DualClauseDatabase(convertedProgram.Statements, new FunctorTableRecord());

        var solver = new CoinductiveSLDSolver(database, new FunctorTableRecord(), _logger);

        foreach (var solution in solver.Solve(convertedProgram.Query))
        {
            PrintSolution(solution);
        }    
    }

    private void InteractiveSolve(List<Statement> rules, List<Goal> query)
    {
        var converter = new ProgramConverter(new FunctorTableRecord(), _logger);
        var convertedRules = rules.Select(converter.ConvertStatement);

        var goalConverter = new GoalConverter(new FunctorTableRecord());
        var convertedQuery = query.Select(x => goalConverter.Convert(x).GetValueOrThrow());

        var database = new DualClauseDatabase(convertedRules, new FunctorTableRecord());

        var solver = new CoinductiveSLDSolver(database, new FunctorTableRecord(), _logger);

        foreach (var solution in solver.Solve(convertedQuery))
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
        Console.WriteLine($"{{ {solution.SolutionSet.Entries.ToList().ListToString()} }}");
        Console.WriteLine("---------------------------------------------------------------");
    }

    static void PrintMapping(VariableMapping mapping)
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
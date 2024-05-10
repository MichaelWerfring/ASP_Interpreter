using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using asp_interpreter_lib.Visitors;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;
using Microsoft.Extensions.Configuration;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Preprocessing.OLONDetection;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Solving.NMRCheck;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        Test1(args);

        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddTransient<Application>();
        builder.Services.AddSingleton(AspExtensions.CommonPrefixes);

        var conf = GetConfig(args);
        if (conf.Help)
        {
            DisplayHelp();
            Console.WriteLine("\nPress any key to close Application...");
            Console.ReadKey();
            return;
        }

        builder.Services.AddSingleton<ILogger>(new ConsoleLogger(conf.LogLevel, conf.Timestamp));
        builder.Services.AddTransient<ProgramVisitor>();
        builder.Services.AddSingleton(conf);

        var host = builder.Build();
        host.Services.GetRequiredService<Application>().Run();
    }
    
    private static ProgramConfig GetConfig(string[] args)
    {
        if(args.Length == 0)
        {
            return new ProgramConfig(" ", false, false, true,  LogLevel.None);
        }

        //Assume that 1 is a path
        if (args.Length == 1)
        {
            return new ProgramConfig(args[0], true, true, false, LogLevel.Debug);
        }

        var parser = InitParser(new ConsoleLogger(LogLevel.Info));
        return parser.Parse(args);
    }

    private static void DisplayHelp()
    {
        Console.WriteLine("How to use: ");

        Console.WriteLine("Options:");
        Console.WriteLine("-p, --path <path>            Specify the path to the input file (mandatory)");
        Console.WriteLine("-l, --log-level < level >    Set the log level(0 to 4: Trace, Debug, Info, Error, None)");
        Console.WriteLine("-t, --timestamp              Log Timestamp for events");
        Console.WriteLine("-h, --help                   Display a help message");
        Console.WriteLine("-i, --interactive            Run in interactive mode");
        Console.WriteLine();

        Console.WriteLine("Examples:");
        Console.WriteLine("interpreter.exe -l 0 -p /path/to/file.txt --interactive");
        Console.WriteLine("interpreter.exe --log-level 1 --path /path/to/file.txt --help");
        Console.WriteLine();

        Console.WriteLine("Dev mode:");
        Console.WriteLine("interpreter.exe /path/to/file.txt");
        Console.WriteLine("is translated to:");
        Console.WriteLine("interpreter.exe -l 1 -p /path/to/file.txt -i");
    }

    private static CommandLineParser InitParser(ILogger logger)
    {
        var actions = new Dictionary<string, Func<int, ProgramConfig, string[], ProgramConfig>>();

        Func<int, ProgramConfig, string[], ProgramConfig> getPath = (i, conf, args) =>
        {
            if (args.Length <= i)
            {
                throw new InvalidOperationException("The parameter for the argument is not contained in the provided array!");
            }

            conf.Path = args[i + 1];

            return conf;
        };
        Func<int, ProgramConfig, string[], ProgramConfig> getLogLevel = (i, conf, args) =>
        {
            if (args.Length <= i)
            {
                throw new InvalidOperationException("The parameter for the argument is not contained in the provided array!");
            }

            if (!Enum.TryParse(args[i + 1], out LogLevel logLevel))
            {
                logger.LogInfo($"The specified log level {args[i + 1]} " +
                    $"was not valid therefore Error(3) has been set as a defaul value!");
                logLevel = LogLevel.Error;
            }

            conf.LogLevel = logLevel;
            return conf;
        };
        Func<int, ProgramConfig, string[], ProgramConfig> getHelp = (i, conf, args) =>
        {
            conf.Help = true;
            return conf;
        };
        Func<int, ProgramConfig, string[], ProgramConfig> getInteractive = (i, conf, args) =>
        {
            conf.Interactive = true;
            return conf;
        };
        Func<int, ProgramConfig, string[], ProgramConfig> getTimestamp = (i, conf, args) =>
        {
            conf.Timestamp = true;
            return conf;
        };



        actions.Add("-p", getPath);
        actions.Add("--path", getPath);

        actions.Add("-l", getLogLevel);
        actions.Add("--log-level", getLogLevel);

        actions.Add("-h", getHelp);
        actions.Add("--help", getHelp);

        actions.Add("-i", getInteractive);
        actions.Add("--interactive", getInteractive);

        actions.Add("-t", getTimestamp);
        actions.Add("--timestamp", getTimestamp);

        return new CommandLineParser(actions);
    }

    static void Test1(string[] args)
    {
        var logger = new ConsoleLogger(LogLevel.Trace);

        var program = ConstructFullProgram(args[0], logger);

        var converter = new ProgramConverter(new FunctorTableRecord(), logger);
        var internalProgram = converter.Convert(program);

        var database = new DualClauseDatabase(internalProgram.Statements, new FunctorTableRecord());

        var appendedQuery = internalProgram.Query.Concat([new Structure("nmr_check", [])]);

        PrintProgram(program, internalProgram.Statements, appendedQuery);

        var sw = new Stopwatch();

        Console.WriteLine(sw.ElapsedMilliseconds.ToString());

        var solver1 = new CoinductiveSLDSolver(database, new FunctorTableRecord(), logger);

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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using asp_interpreter_lib.Visitors;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;
using Microsoft.Extensions.Configuration;
using asp_interpreter_exe;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddTransient<Application>();
        builder.Services.AddSingleton(AspExtensions.CommonPrefixes);

        var conf = GetConfig(args);
        if (conf.Help)
        {
            DisplayHelp();
            return;
        }

        builder.Services.AddSingleton<ILogger>(new ConsoleLogger(conf.LogLevel));
        builder.Services.AddTransient<ProgramVisitor>();
        builder.Services.AddSingleton(conf);

        var host = builder.Build();
        host.Services.GetRequiredService<Application>().Run();

        

    }
    private static ProgramConfig GetConfig(string[] args)
    {
        //Assume that 1 is a path
        if (args.Length == 1)
        {
            return new ProgramConfig(args[0], true, false, LogLevel.Debug);
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
        Console.WriteLine("-h, --help                   Display a help message");
        Console.WriteLine("-i, --interactive            Run in interactive mode");
        Console.WriteLine();

        Console.WriteLine("Examples:");
        Console.WriteLine("interpreter -l 0 -p /path/to/file.txt --interactive");
        Console.WriteLine("interpreter --log-level 1 --path /path/to/file.txt --help");
        Console.WriteLine();

        Console.WriteLine("Dev mode:");
        Console.WriteLine("interpreter /path/to/file.txt");
        Console.WriteLine("is translated to:");
        Console.WriteLine("interpreter -l 1 -p /path/to/file.txt -i");
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

        actions.Add("-p", getPath);
        actions.Add("--path", getPath);

        actions.Add("-l", getLogLevel);
        actions.Add("--log-level", getLogLevel);

        actions.Add("-h", getHelp);
        actions.Add("--help", getHelp);

        actions.Add("-i", getInteractive);
        actions.Add("--interactive", getInteractive);

        return new CommandLineParser(actions);
    }
}
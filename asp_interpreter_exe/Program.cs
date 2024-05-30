//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

using Asp_interpreter_exe;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.Visitors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Represents a program.
/// </summary>
internal class Program
{
    /// <summary>
    /// Entry point of the program.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddTransient<Application>();
        builder.Services.AddSingleton(AspExtensions.CommonPrefixes);

        var conf = GetConfig(args);
        if (conf.DisplayHelp)
        {
            DisplayHelp();
            Console.WriteLine("\nPress any key to close Application...");
            Console.ReadKey();
            return;
        }

        builder.Services.AddSingleton<ILogger>(new ConsoleLogger(conf.LogLevel, conf.LogTimestamp));
        builder.Services.AddTransient<ProgramVisitor>();
        builder.Services.AddSingleton(conf);

        var host = builder.Build();
        host.Services.GetRequiredService<Application>().Run();
    }

    private static ProgramConfig GetConfig(string[] args)
    {
        if (args.Length == 0)
        {
            return new ProgramConfig(" ", false, false, false, true, LogLevel.None);
        }

        // Assume that 1 is a path
        if (args.Length == 1)
        {
            return new ProgramConfig(args[0], false, true, true, false, LogLevel.Debug);
        }

        var tempLogger = new ConsoleLogger(LogLevel.Info);
        var parser = InitParser(tempLogger);
        var conf = parser.Parse(args);

        if (string.IsNullOrEmpty(conf.FilePath))
        {
            tempLogger.LogError("The path to the file was not provided correctly!");
            return new ProgramConfig(" ", false, false, false, true, LogLevel.None);
        }

        return conf;
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
        Console.WriteLine("-e, --explain                Explain the program");
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

            conf.FilePath = args[i + 1];

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
            conf.DisplayHelp = true;
            return conf;
        };
        Func<int, ProgramConfig, string[], ProgramConfig> getInteractive = (i, conf, args) =>
        {
            conf.RunInteractive = true;
            return conf;
        };
        Func<int, ProgramConfig, string[], ProgramConfig> getTimestamp = (i, conf, args) =>
        {
            conf.LogTimestamp = true;
            return conf;
        };
        Func<int, ProgramConfig, string[], ProgramConfig> getExplain = (i, conf, args) =>
        {
            conf.DisplayExplanation = true;
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

        actions.Add("-e", getExplain);
        actions.Add("--explain", getExplain);

        return new CommandLineParser(actions);
    }
}
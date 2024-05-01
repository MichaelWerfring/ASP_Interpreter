using Antlr4.Runtime;
using Microsoft.Extensions.Logging;

namespace asp_interpreter_lib.Util.ErrorHandling;

public class ConsoleLogger(LogLevel logLevel) : ILogger
{
    public void LogTrace(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));

        if (logLevel <= LogLevel.Trace)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"Trace: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }

    public void LogDebug(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));

        if (logLevel <= LogLevel.Debug)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Debug: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }

    public void LogInfo(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));

        if (logLevel <= LogLevel.Info)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"Info: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }

    public void LogError(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));

        if (logLevel <= LogLevel.Error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Error: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }

    public void LogError(string message, ParserRuleContext context)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
        ArgumentNullException.ThrowIfNull(context);
              
        if (logLevel <= LogLevel.Error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Error: ");
            Console.ResetColor();
            Console.WriteLine($"{message} at line {context.Start.Line} column {context.Start.Column}");
        }
    }

    public ILogger GetDummy()
    {
        return new ConsoleLogger(LogLevel.None);
    }
}
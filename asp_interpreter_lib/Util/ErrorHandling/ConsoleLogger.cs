﻿using System.Diagnostics;
using System.Globalization;
using Antlr4.Runtime;
using Microsoft.Extensions.Logging;

namespace Asp_interpreter_lib.Util.ErrorHandling;

public class ConsoleLogger(LogLevel logLevel, bool logTimestamp = false) : ILogger
{
    public void LogTrace(string message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        if (logLevel <= LogLevel.Trace)
        {
            if (logTimestamp) LogTimestamp();
            
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"Trace: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }
    
    public void LogDebug(string message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        
        if (logLevel <= LogLevel.Debug)
        {
            if (logTimestamp) LogTimestamp();
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Debug: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }

    public void LogInfo(string message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        if (logLevel <= LogLevel.Info)
        {
            if (logTimestamp) LogTimestamp();
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"Info: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }

    public void LogError(string message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        if (logLevel <= LogLevel.Error)
        {
            if (logTimestamp) LogTimestamp();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Error: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }

    public void LogError(string message, ParserRuleContext context)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        ArgumentNullException.ThrowIfNull(context);
              
        if (logLevel <= LogLevel.Error)
        {
            if (logTimestamp) LogTimestamp();
            
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
    
    private static void LogTimestamp()
    {
        Console.Write(DateTime.Now.Hour + ":" +
                      DateTime.Now.Minute + ":" +
                      DateTime.Now.Second + " " +
                      DateTime.Now.Millisecond + "ms ");
    }
}
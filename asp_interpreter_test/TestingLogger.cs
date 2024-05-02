using System.Runtime.InteropServices.JavaScript;
using Antlr4.Runtime;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_test;

public class TestingLogger(LogLevel logLevel) : ILogger
{
    public List<string> ErrorMessages { get; } = [];
    
    public List<string> DebugMessages { get; } = [];
    
    public List<string> TraceMessages { get; } = [];
    
    public List<string> InfoMessages { get; } = [];
    
    public List<string> Errors { get; } = [];

    public LogLevel LogLevel { get; } = logLevel;

    public void LogError(string message, ParserRuleContext _)
    {
        if(LogLevel == LogLevel.Error)
        {
            ErrorMessages.Add(message);
        }
    }

    public void LogTrace(string message)
    {
        if (LogLevel == LogLevel.Trace)
        {
            TraceMessages.Add(message);
        }
    }

    public void LogDebug(string message)
    {
        if (LogLevel == LogLevel.Debug)
        {
            DebugMessages.Add(message);
        }
    }

    public void LogInfo(string message)
    {
        if (LogLevel == LogLevel.Info)
        {
            InfoMessages.Add(message);
        }
    }

    public void LogError(string message)
    {
        if (LogLevel == LogLevel.Error)
        {
            ErrorMessages.Add(message);
        }
    }

    public ILogger GetDummy()
    {
        return new TestingLogger(LogLevel.None);
    }
}
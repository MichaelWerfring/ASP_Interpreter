using System.Runtime.InteropServices.JavaScript;
using Antlr4.Runtime;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_test;

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
        if(this.LogLevel <= LogLevel.Error)
        {
            this.ErrorMessages.Add(message);
        }
    }

    public void LogTrace(string message)
    {
        if (this.LogLevel <= LogLevel.Trace)
        {
            this.TraceMessages.Add(message);
        }
    }

    public void LogDebug(string message)
    {
        if (this.LogLevel <= LogLevel.Debug)
        {
            this.DebugMessages.Add(message);
        }
    }

    public void LogInfo(string message)
    {
        if (this.LogLevel <= LogLevel.Info)
        {
            this.InfoMessages.Add(message);
        }
    }

    public void LogError(string message)
    {
        if (this.LogLevel <= LogLevel.Error)
        {
            this.ErrorMessages.Add(message);
        }
    }

    public ILogger GetDummy()
    {
        return new TestingLogger(LogLevel.None);
    }
}
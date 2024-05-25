using Antlr4.Runtime;

namespace Asp_interpreter_lib.Util.ErrorHandling;

public interface ILogger
{
    void LogTrace(string message);
    
    void LogDebug(string message);

    void LogInfo(string message);

    void LogError(string message);

    void LogError(string message, ParserRuleContext context);

    ILogger GetDummy();
}
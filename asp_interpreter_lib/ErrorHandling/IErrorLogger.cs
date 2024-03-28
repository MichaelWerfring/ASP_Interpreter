using Antlr4.Runtime;

namespace asp_interpreter_lib.ErrorHandling;

public interface IErrorLogger
{
    void LogError(string message, ParserRuleContext context);
}
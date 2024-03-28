using Antlr4.Runtime;

namespace asp_interpreter_lib.ErrorHandling;

public class ConsoleErrorLogger : IErrorLogger
{
    public void LogError(string message, ParserRuleContext context)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(context);
        
        Console.WriteLine($"Error: {message} at line {context.Start.Line} column {context.Start.Column}");
    }
}
using Antlr4.Runtime;
using asp_interpreter_lib.ErrorHandling;

namespace asp_interpreter_test;

public struct Error
{
    public string Message { get; set; }
    public ParserRuleContext Context { get; set; }
}

public class MockErrorLogger : IErrorLogger
{
    private List<Error> _errors = [];
    
    public void LogError(string message, ParserRuleContext context)
    {
        _errors.Add(new Error { Message = message, Context = context });
    }
}
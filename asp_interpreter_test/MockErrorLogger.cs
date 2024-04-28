using System.Runtime.InteropServices.JavaScript;
using Antlr4.Runtime;
using asp_interpreter_lib.Util.ErrorHandling;

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
        Errors.Add(new Error { Message = message, Context = context });
    }

    public List<Error> Errors => _errors;
}
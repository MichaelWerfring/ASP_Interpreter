using asp_interpreter_lib;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_test;

public class TermVisitorTest
{
    private readonly ILogger _errorLogger = new ConsoleLogger();
    
    [Test]
    public void ParseVariableTerm()
    {
        string code = "a(X). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();
        
        Assert.That(literal != null &&
            literal.Terms[0] is VariableTerm &&
            literal.Terms[0].ToString() == "X");
    }
    
    [Test]
    public void ParseStringTerm()
    {
        string code = "a(\"hallo\"). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();
        
        Assert.That(literal != null &&
            literal.Terms[0] is StringTerm &&
            literal.Terms[0].ToString() == "hallo");
    }
    
    [Test]
    public void ParseBasicTerm()
    {
        string code = "a(b, c). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();
        
        Assert.That(literal != null &&
            literal.Terms[0] is BasicTerm && literal.Terms[1] is BasicTerm &&
            literal.Terms[0].ToString() == "b" && 
            literal.Terms[1].ToString() == "c");
    }
    
    [Test]
    public void ParseNegatedTerm()
    {
        string code = "a(-1). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);
        var converter = new TermToNumberConverter();
        
        var literal = program.Statements[0].Head.GetValueOrThrow();
        var term = literal?.Terms[0];
        var content = term?.Accept(converter);
        
        Assert.That(content != null &&
            content.HasValue && content.GetValueOrThrow() == -1);
    }
    
    [Test]
    public void ParseArithmeticOperationTerm()
    {
        string code = "a(1 + 2). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();
        
        Assert.That(literal != null &&
            literal.Terms[0] is ArithmeticOperationTerm &&
            literal.Terms[0].ToString() == "1 + 2");
    }
    
    
    [Test]
    public void ParseBasicTermWithInnerTerms()
    {
        string code = "a(b, c(d, e)). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();
        
        Assert.That(literal != null &&
            literal.Terms[0] is BasicTerm && literal.Terms[1] is BasicTerm &&
            literal.Terms[0].ToString() == "b" && 
            literal.Terms[1].ToString() == "c(d, e)");
    }
    
    [Test]
    public void ParseArithmeticOperationTermWithInnerTerms()
    {
        string code = "a(1 + 2 * 3). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();
        
        Assert.That(literal != null &&
            literal.Terms[0] is ArithmeticOperationTerm &&
            literal.Terms[0].ToString() == "1 + 2 * 3");
    }
    
    [Test]
    public void ParseParenthesizedTerm()
    {
        string code = "a((b)). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();
        
        Assert.That(literal != null &&
            literal.Terms[0] is BasicTerm &&
            literal.Terms[0].ToString() == "b");
    }
    
    [Test]
    public void ParseParenthesizedTermWithMultipleInnerTerms()
    {
        string code = "a(b,(c(d, e, f, g))). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();
        
        Assert.That(literal != null &&
            literal.Terms[0] is BasicTerm && literal.Terms[1] is BasicTerm &&
            literal.Terms[0].ToString() == "b" && 
            literal.Terms[1].ToString() == "c(d, e, f, g)");
    }
    
    [Test]
    public void ParseAnonymusVariableTerm()
    {
        string code = "a(_). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();
        
        Assert.That(literal != null &&
            literal.Terms[0] is AnonymusVariableTerm &&
            literal.Terms[0].ToString() == "_");
    }

    [Test]
    public void ParseAnonymusVariableTermWithSeveralArguments()
    {
        string code = "a(b, _). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();
        
        Assert.That(literal != null &&
            literal.Terms[0] is BasicTerm && literal.Terms[1] is AnonymusVariableTerm &&
            literal.Terms[0].ToString() == "b" && 
            literal.Terms[1].ToString() == "_");
    }
    
    [Test]
    public void ParseAnonymusVariableTermWithInnerTerms()
    {
        string code = "a(b, c(d, _)). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();
        
        Assert.That(literal != null &&
            literal.Terms[0] is BasicTerm && literal.Terms[1] is BasicTerm &&
            literal.Terms[0].ToString() == "b" && 
            literal.Terms[1].ToString() == "c(d, _)");
    }
    
    [Test]
    public void ParseNumberTerm()
    {
        string code = "a(1). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);
        var converter = new TermToNumberConverter();

        var literal = program.Statements[0].Head.GetValueOrThrow();
        var term = literal?.Terms[0];
        var content = term?.Accept(converter);
        
        Assert.That(content != null &&
            content.HasValue && content.GetValueOrThrow() == 1);
    }
    
    [Test]
    public void ParseNumberTermWithSeveralArguments()
    {
        string code = "a(1, 2, 3, 4, 5). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);
        var converter = new TermToNumberConverter();

        var literal = program.Statements[0].Head.GetValueOrThrow();
        var term = literal?.Terms[1];
        var content = term?.Accept(converter);
        
        Assert.That(content != null &&
            content.HasValue && content.GetValueOrThrow() == 2);
    }
    
    [Test]
    public void ParseNumberTermWithInnerTerms()
    {
        string code = "a(1, 2, 3, 7). a?";
        var program = AspExtensions.GetProgram(code, _errorLogger);
        var converter = new TermToNumberConverter();

        var literal = program.Statements[0].Head.GetValueOrThrow();
        var term = literal?.Terms[2];
        var content = term?.Accept(converter);
        
        Assert.That(content != null &&
            content.HasValue && content.GetValueOrThrow() == 3);
    }
}
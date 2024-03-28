using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_test;

public class TermTest
{
    private TermToNumberConverter _visitor = new TermToNumberConverter();
    
    [Test]
    public void TermToNumberConverterFailsOnBasicTerm()
    {
        Term term = new BasicTerm("a", []);
        var result = term.Accept(_visitor);
        
        Assert.That(!result.HasValue);
    }
    
    [Test]
    public void TermToNumberConverterFailsOnAnonymusVariableTerm()
    {
        Term term = new AnonymusVariableTerm();
        var result = term.Accept(_visitor);
        
        Assert.That(!result.HasValue);
    }
    
    [Test]
    public void TermToNumberConverterFailsOnVariableTerm()
    {
        Term term = new VariableTerm("a");
        
        Assert.Throws<NotImplementedException>(() => term.Accept(_visitor));
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnPlus()
    {
        Term term = new ArithmeticOperationTerm(new Plus(new NumberTerm(1), new NumberTerm(1)));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 2);
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnMinus()
    {
        Term term = new ArithmeticOperationTerm(new Minus(new NumberTerm(1), new NumberTerm(1)));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 0);
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnTimes()
    {
        Term term = new ArithmeticOperationTerm(new Multiply(new NumberTerm(2), new NumberTerm(2)));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 4);
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnDivide()
    {
        Term term = new ArithmeticOperationTerm(new Divide(new NumberTerm(4), new NumberTerm(2)));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 2);
    }
    
    [Test]
    public void TermToNumberConverterSuccessOnParenthesizedTerm()
    {
        Term term = new ParenthesizedTerm(new NumberTerm(1));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 1);
    }
    
    [Test]
    public void TermToNumberConverterFailsOnStringTerm()
    {
        Term term = new StringTerm("a");
        var result = term.Accept(_visitor);
        
        Assert.That(!result.HasValue);
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnNegatedTerm()
    {
        Term term = new NegatedTerm(new NumberTerm(1));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == -1);
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnConventionalNumberTerm()
    {
        Term term = new NumberTerm(1);
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 1);
    }
}
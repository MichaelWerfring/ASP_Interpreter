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
        ITerm term = new BasicTerm("a", []);
        var result = term.Accept(_visitor);
        
        Assert.That(!result.HasValue);
    }
    
    [Test]
    public void TermToNumberConverterFailsOnAnonymusVariableTerm()
    {
        ITerm term = new AnonymusVariableTerm();
        var result = term.Accept(_visitor);
        
        Assert.That(!result.HasValue);
    }
    
    [Test]
    public void TermToNumberConverterFailsOnVariableTerm()
    {
        ITerm term = new VariableTerm("a");
        
        Assert.Throws<NotImplementedException>(() => term.Accept(_visitor));
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnPlus()
    {
        ITerm term = new ArithmeticOperationTerm(new Plus(new NumberTerm(1), new NumberTerm(1)));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 2);
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnMinus()
    {
        ITerm term = new ArithmeticOperationTerm(new Minus(new NumberTerm(1), new NumberTerm(1)));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 0);
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnTimes()
    {
        ITerm term = new ArithmeticOperationTerm(new Multiply(new NumberTerm(2), new NumberTerm(2)));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 4);
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnDivide()
    {
        ITerm term = new ArithmeticOperationTerm(new Divide(new NumberTerm(4), new NumberTerm(2)));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 2);
    }
    
    [Test]
    public void TermToNumberConverterSuccessOnParenthesizedTerm()
    {
        ITerm term = new ParenthesizedTerm(new NumberTerm(1));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 1);
    }
    
    [Test]
    public void TermToNumberConverterFailsOnStringTerm()
    {
        ITerm term = new StringTerm("a");
        var result = term.Accept(_visitor);
        
        Assert.That(!result.HasValue);
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnNegatedTerm()
    {
        ITerm term = new NegatedTerm(new NumberTerm(1));
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == -1);
    }
    
    [Test]
    public void TermToNumberConverterSucceedsOnConventionalNumberTerm()
    {
        ITerm term = new NumberTerm(1);
        var result = term.Accept(_visitor);
        
        Assert.That(result.HasValue && result.GetValueOrThrow() == 1);
    }
}
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_test;

public class BinaryOperationsTest
{
    [Test]
    public void EqualitySucceedsOnEqualNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(1),
            new Equality(),
            new NumberTerm(1));
        
        Assert.True(term.Evaluate());
    }
    
    [Test]
    public void EqualityFailsOnDifferentNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(1),
            new Equality(),
            new NumberTerm(2));
        
        Assert.False(term.Evaluate());
    }
    
    [Test]
    public void GreaterThanSucceedsOnGreaterNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(2),
            new GreaterThan(),
            new NumberTerm(1));
        
        Assert.True(term.Evaluate());
    }
    
    [Test]
    public void GreaterThanFailsOnEqualNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(1),
            new GreaterThan(),
            new NumberTerm(1));
        
        Assert.False(term.Evaluate());
    }
    
    [Test]
    public void GreaterThanFailsOnSmallerNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(1),
            new GreaterThan(),
            new NumberTerm(2));
        
        Assert.False(term.Evaluate());
    }
    
    [Test]
    public void LessThanSucceedsOnSmallerNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(1),
            new LessThan(),
            new NumberTerm(2));
        
        Assert.True(term.Evaluate());
    }
    
    [Test]
    public void LessThanFailsOnEqualNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(1),
            new LessThan(),
            new NumberTerm(1));
        
        Assert.False(term.Evaluate());
    }
    
    [Test]
    public void LessThanFailsOnGreaterNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(2),
            new LessThan(),
            new NumberTerm(1));
        
        Assert.False(term.Evaluate());
    }
    
    [Test]
    public void GreaterOrEqualSucceedsOnGreaterNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(2),
            new GreaterOrEqualThan(),
            new NumberTerm(1));
        
        Assert.True(term.Evaluate());
    }
    
    [Test]
    public void GreaterOrEqualSucceedsOnEqualNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(1),
            new GreaterOrEqualThan(),
            new NumberTerm(1));
        
        Assert.True(term.Evaluate());
    }
    
    [Test]
    public void GreaterOrEqualFailsOnSmallerNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(1),
            new GreaterOrEqualThan(),
            new NumberTerm(2));
        
        Assert.False(term.Evaluate());
    }
    
    [Test]
    public void LessOrEqualSucceedsOnSmallerNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(1),
            new LessOrEqualThan(),
            new NumberTerm(2));
        
        Assert.True(term.Evaluate());
    }
    
    [Test]
    public void LessOrEqualSucceedsOnEqualNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(1),
            new LessOrEqualThan(),
            new NumberTerm(1));
        
        Assert.True(term.Evaluate());
    }
    
    [Test]
    public void LessOrEqualFailsOnGreaterNumbers()
    {
        var term = new BinaryOperation(
            new NumberTerm(2),
            new LessOrEqualThan(),
            new NumberTerm(1));
        
        Assert.False(term.Evaluate());
    }
}
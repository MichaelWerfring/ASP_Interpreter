using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_test;

public class ArithmeticOperationTest
{
    [TestCase(0, 0)]
    [TestCase(-1, 1)]
    [TestCase(1, -1)]
    [TestCase(int.MaxValue, int.MinValue + 1)]
    [TestCase(int.MinValue + 1, int.MaxValue)]
    public void TestTermNegationOnNumbers(int number, int expected)
    {
        var innerTerm = new NumberTerm(number);
        var term = new NegatedTerm(innerTerm);

        var converter = new TermToNumberConverter();
        var actual = term.Accept(converter);
        
        Assert.That(actual.HasValue && actual.GetValueOrThrow() == expected);
    }
    
    [TestCase(5, 5, 10)]
    [TestCase(5, -5, 0)]
    [TestCase(-5, 5, 0)]
    [TestCase(-5, -5, -10)]
    public void AdditionOnNumbersWorks(int left, int right, int expected)
    {
        var term = new Plus(new NumberTerm(left), new NumberTerm(right));
        ITerm result = term.Evaluate();
        
        var converter = new TermToNumberConverter();
        var actual = result.Accept(converter);
        
        Assert.That(actual.HasValue && actual.GetValueOrThrow() == expected);
    }
    
    [TestCase(5, 5, 25)]
    [TestCase(5, -5, -25)]
    [TestCase(-5, 5, -25)]
    [TestCase(-5, -5, 25)]
    public void MultiplicationOnNumbersWorks(int left, int right, int expected)
    {
        var term = new Multiply(new NumberTerm(left), new NumberTerm(right));
        ITerm result = term.Evaluate();
        
        var converter = new TermToNumberConverter();
        var actual = result.Accept(converter);
        
        Assert.That(actual.HasValue && actual.GetValueOrThrow() == expected);
    }
    
    [TestCase(5, 5, 0)]
    [TestCase(5, -5, 10)]
    [TestCase(-5, 5, -10)]
    [TestCase(-5, -5, 0)]
    public void SubtractionOnNumbersWorks(int left, int right, int expected)
    {
        var term = new Minus(new NumberTerm(left), new NumberTerm(right));
        ITerm result = term.Evaluate();
        
        var converter = new TermToNumberConverter();
        var actual = result.Accept(converter);
        
        Assert.That(actual.HasValue && actual.GetValueOrThrow() == expected);
    }
    
    [TestCase(5, 5, 1)]
    [TestCase(5, -5, -1)]
    [TestCase(-5, 5, -1)]
    [TestCase(-5, -5, 1)]
    public void DivisionOnNumbersWorks(int left, int right, int expected)
    {
        var term = new Divide(new NumberTerm(left), new NumberTerm(right));
        ITerm result = term.Evaluate();
        
        var converter = new TermToNumberConverter();
        var actual = result.Accept(converter);
        
        Assert.That(actual.HasValue && actual.GetValueOrThrow() == expected);
    }
    
    [Test]
    public void DivisionOnZeroThrowsException()
    {
        var term = new Divide(new NumberTerm(5), new NumberTerm(0));
        
        Assert.Throws<DivideByZeroException>(() => term.Evaluate());
    }
    
    [Test]
    public void AdditionOnNumberAndStringThrowsException()
    {
        var term = new Plus(new NumberTerm(5), new StringTerm("a"));
        
        Assert.Throws<InvalidOperationException>(() => term.Evaluate());
    }
    
    [Test]
    public void MultiplicationOnNumberAndStringThrowsException()
    {
        var term = new Multiply(new NumberTerm(5), new StringTerm("a"));
        
        Assert.Throws<InvalidOperationException>(() => term.Evaluate());
    }
    
    [Test]
    public void SubtractionOnNumberAndStringThrowsException()
    {
        var term = new Minus(new NumberTerm(5), new StringTerm("a"));
        
        Assert.Throws<InvalidOperationException>(() => term.Evaluate());
    }
    
    [Test]
    public void DivisionOnNumberAndStringThrowsException()
    {
        var term = new Divide(new NumberTerm(5), new StringTerm("a"));
        
        Assert.Throws<InvalidOperationException>(() => term.Evaluate());
    }
    
    [Test]
    public void AdditionOnStringAndNumberThrowsException()
    {
        var term = new Plus(new StringTerm("a"), new NumberTerm(5));
        
        Assert.Throws<InvalidOperationException>(() => term.Evaluate());
    }
    
    [Test]
    public void MultiplicationOnStringAndNumberThrowsException()
    {
        var term = new Multiply(new StringTerm("a"), new NumberTerm(5));
        
        Assert.Throws<InvalidOperationException>(() => term.Evaluate());
    }
    
    [Test]
    public void SubtractionOnStringAndNumberThrowsException()
    {
        var term = new Minus(new StringTerm("a"), new NumberTerm(5));
        
        Assert.Throws<InvalidOperationException>(() => term.Evaluate());
    }
    
    [Test]
    public void DivisionOnStringAndNumberThrowsException()
    {
        var term = new Divide(new StringTerm("a"), new NumberTerm(5));
        
        Assert.Throws<InvalidOperationException>(() => term.Evaluate());
    }
    
    [Test]
    public void AdditionToStringReturnsCorrectString()
    {
        var term = new Plus(new NumberTerm(1), new NumberTerm(2));
        
        Assert.That(term.ToString() == "1 + 2");
    }
    
    [Test]
    public void MultiplicationToStringReturnsCorrectString()
    {
        var term = new Multiply(new NumberTerm(1), new NumberTerm(2));
        
        Assert.That(term.ToString() == "1 * 2");
    }
    
    [Test]
    public void SubtractionToStringReturnsCorrectString()
    {
        var term = new Minus(new NumberTerm(1), new NumberTerm(2));
        
        Assert.That(term.ToString() == "1 - 2");
    }
    
    [Test]
    public void DivisionToStringReturnsCorrectString()
    {
        var term = new Divide(new NumberTerm(1), new NumberTerm(2));
        
        Assert.That(term.ToString() == "1 / 2");
    }
}
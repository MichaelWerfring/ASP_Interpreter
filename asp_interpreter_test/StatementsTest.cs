using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_test;

public class StatementsTest
{
    [Test]
    public void CreatesEmptyStatementPerDefault()
    {
        var statement = new Statement();
        
        Assert.That(statement is { HasBody: false, HasHead: false});
    }
    
    [Test]
    public void AddsEmptyBodyCorrectly()
    {
        var statement = new Statement();
        List<Goal> body = [];
        
        statement.AddBody(body);
        
        Assert.That(statement is { HasBody: false, HasHead: false});
    }
    
    [Test]
    public void AddsHeadCorrectly()
    {
        var statement = new Statement();
        var head = new Literal("a", false,false, []);
        
        statement.AddHead(head);
        
        Assert.That(statement is { HasBody: false, HasHead: true});
    }
    
    [Test]
    public void AddsBodyCorrectly()
    {
        var statement = new Statement();
        var literal = new Literal("b",true, false, []);
        List<Goal> body = [literal];
        
        statement.AddBody(body);
        
        Assert.That(statement is { HasBody: true, HasHead: false});
    }
    
    [Test]
    public void AllowsAddingHeadAndBody()
    {
        var statement = new Statement();
        var head = new Literal("a", false,false, []);
        var literal = new Literal("b", true,false, []);
        List<Goal> body = [literal];
        
        statement.AddHead(head);
        statement.AddBody(body);
        
        Assert.That(statement is { HasBody: true, HasHead: true});
    }
    
    [Test]
    public void ThrowsExceptionWhenAddingHeadTwice()
    {
        var statement = new Statement();
        var head = new Literal("a", false,false, []);
        
        statement.AddHead(head);
        
        Assert.Throws<ArgumentException>(() => statement.AddHead(head));
    }
    
    [Test]
    public void ThrowsExceptionWhenAddingBodyTwice()
    {
        var statement = new Statement();
        var literal = new Literal("b", true, false, []);
        List<Goal> body = [literal];
        
        statement.AddBody(body);
        
        Assert.Throws<ArgumentException>(() => statement.AddBody(body));
    }
    
    [Test]
    public void ThrowsExceptionWhenAddingNullHead()
    {
        var statement = new Statement();
        
        Assert.Throws<ArgumentNullException>(() => statement.AddHead(null));
    }
    
    [Test]
    public void ThrowsExceptionWhenAddingNullBody()
    {
        var statement = new Statement();
        
        Assert.Throws<ArgumentNullException>(() => statement.AddBody(null));
    }
}
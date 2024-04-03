using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using NuGet.Frameworks;
using Is = NUnit.Framework.Is;

namespace asp_interpreter_test;

public class DualRuleTest
{
    [Test]
    public void TransformHeadIgnoresEmptyHead()
    {
        var rule = new Statement();
        rule.AddHead(new Head());
        
        var result = DualRuleConverter.ReplaceDuplicateVariables(rule);
        Assert.That(result, Is.EqualTo(rule));
    }
    
    [Test]
    public void TransformHeadIgnoresNonVariableTerms()
    {
        var rule = new Statement();
        var terms = new List<ITerm>
        {
            new NumberTerm(1),
            new BasicTerm("a", []),
            new NumberTerm(2),
            new BasicTerm("b", []),
        };
        
        rule.AddHead(new Head(new ClassicalLiteral("test", false, terms)));
        
        var result = DualRuleConverter.ReplaceDuplicateVariables(rule);
        Assert.That(result, Is.EqualTo(rule));
    }
    
    [Test]
    public void TransformHeadIgnoresDistinctVariables()
    {
        var rule = new Statement();
        var terms = new List<ITerm>
        {
            new BasicTerm("a", []),
            new VariableTerm("A"),
            new BasicTerm("a", []),
            new VariableTerm("D"),
            new VariableTerm("E")
        };
        
        rule.AddHead(new Head(new ClassicalLiteral("test", false, terms)));
        
        var result = DualRuleConverter.ReplaceDuplicateVariables(rule);
        
        Assert.That(result, Is.EqualTo(rule));
    }
    
    [Test]
    public void TransformHeadRewritesDuplicateVariables()
    {
        var rule = new Statement();
        var terms = new List<ITerm>
        {
            new BasicTerm("a", []),
            new VariableTerm("A"),
            new VariableTerm("A"),
            new BasicTerm("a", []),
            new VariableTerm("B")
        };
        
        rule.AddHead(new Head(new ClassicalLiteral("test", false, terms)));
        
        var result = DualRuleConverter.ReplaceDuplicateVariables(rule).ToString();
       
        //Assert.That(result, Is.EqualTo("test(a, A, rwh0_A, a, B) :- A = rwh0_A."));
        Assert.That(result, Is.EqualTo("test(rwh0_a, A, rwh0_A, rwh1_a, B) :- rwh1_a = a, A = rwh0_A, rwh0_a = a."));
    }
    
    [Test]
    public void TransformHeadDoesNotAlterRuleBody()
    {
        var rule = new Statement();
        var terms = new List<ITerm>
        {
            new BasicTerm("a", []),
            new VariableTerm("A"),
            new VariableTerm("A"),
            new BasicTerm("a", []),
            new VariableTerm("B")
        };
        
        rule.AddHead(new Head(new ClassicalLiteral("test", false, terms)));
        
        var operation = new BinaryOperation(
            new VariableTerm("A"),
            new Equality(),
            new VariableTerm("B"));
        var body = new Body([new NafLiteral(operation)]);
        
        rule.AddBody(body);
        
        
        var result = DualRuleConverter.ReplaceDuplicateVariables(rule).ToString();
       
        //Assert.That(result, Is.EqualTo("test(a, A, rwh0_A, a, B) :- A = B, A = rwh0_A."));
        Assert.That(result, Is.EqualTo("test(rwh0_a, A, rwh0_A, rwh1_a, B) :- rwh1_a = a, A = rwh0_A, rwh0_a = a, A = B."));
    }
    
    [Test]
    public void TransformHeadRewritesDuplicateMultipleOccurrences()
    {
        var rule = new Statement();
        var terms = new List<ITerm>
        {
            new BasicTerm("a", []),
            new VariableTerm("A"),
            new VariableTerm("A"),
            new BasicTerm("a", []),
            new VariableTerm("A"),
            new VariableTerm("B")
        };
        
        rule.AddHead(new Head(new ClassicalLiteral("test", false, terms)));
        
        var result = DualRuleConverter.ReplaceDuplicateVariables(rule).ToString();
        
        
        Assert.That(result, Is.EqualTo(
            "test(rwh0_a, A, rwh0_A, rwh1_a, rwh1_A, B) :- A = rwh1_A, rwh1_a = a, A = rwh0_A, rwh0_a = a."));
    }
    
    [Test]
    public void TransformHeadRewritesDuplicateMultipleOccurrencesOfDifferentVariables()
    {
        var rule = new Statement();
        var terms = new List<ITerm>
        {
            new VariableTerm("B"),
            new VariableTerm("A"),
            new BasicTerm("b", []),
            new BasicTerm("b", []),
            new VariableTerm("A"),
            new BasicTerm("a", []),
            new VariableTerm("B"),
            new VariableTerm("A"),
            new VariableTerm("B")
        };
        
        rule.AddHead(new Head(new ClassicalLiteral("test", false, terms)));
        
        var result = DualRuleConverter.ReplaceDuplicateVariables(rule).ToString();
        var expected =
            "test(B, A, rwh0_b, rwh1_b, rwh0_A, rwh0_a, rwh0_B, rwh1_A, rwh1_B) :- " +
            "B = rwh1_B, A = rwh1_A, B = rwh0_B, rwh0_a = a, A = rwh0_A, rwh1_b = b, rwh0_b = b.";
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void GetDualRulesForStatementReturnsCorrectDualRules()
    {
        var rule = new Statement();
        
        //p(X, Y) :- not q(X), t(Y, Y).
        List<ITerm> headTerms = new List<ITerm> { new VariableTerm("X"), new VariableTerm("Y") };
        rule.AddHead(new Head(new ClassicalLiteral("p", false ,headTerms)));

        List<NafLiteral> body = new List<NafLiteral>()
        {
            new NafLiteral(new ClassicalLiteral("q", false, [new VariableTerm("X")]),true),
            new NafLiteral(new ClassicalLiteral("t",false, [new VariableTerm("Y"), new VariableTerm("Y")]),false),
        };
        
        rule.AddBody(new Body(body));


        var result = DualRuleConverter.GetDualRules(rule);

        //not p(X, Y) :- q(X).
        string firstDual = result[0].ToString();
        
        //not p(X, Y) :- not q(X), not t(Y, Y).
        string secondDual = result[1].ToString();
        
        Assert.That(firstDual == "p(X, Y) :- q(X)." && secondDual == "p(X, Y) :- not q(X), not t(Y, Y).");
    }
}
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_test;

public class DualRuleTest
{
    [Test]
    public void TransformHeadIgnoresEmptyHead()
    {
        var rule = new Statement();
        rule.AddHead(new Head());
        
        var result = DualRuleConverter.TransformHead(rule);
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
        
        var result = DualRuleConverter.TransformHead(rule);
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
        
        var result = DualRuleConverter.TransformHead(rule);
        
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
        
        var result = DualRuleConverter.TransformHead(rule).ToString();
       
        Assert.That(result, Is.EqualTo("test(a, A, rwh0_A, a, B) :- A = rwh0_A."));
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
        
        var result = DualRuleConverter.TransformHead(rule).ToString();
        
        Assert.That(result, Is.EqualTo("test(a, A, rwh0_A, a, rwh1_A, B) :- A = rwh0_A, A = rwh1_A."));
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
        
        var result = DualRuleConverter.TransformHead(rule).ToString();
        
        Assert.That(result, Is.EqualTo(
            "test(B, A, b, b, rwh0_A, a, rwh0_B, rwh1_A, rwh1_B) :- A = rwh0_A, B = rwh0_B, A = rwh1_A, B = rwh1_B."));
    }
}
using System.Xml.Linq;
using asp_interpreter_lib;
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
        
        var result = DualRuleConverter.ComputeHead(rule);
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
        
        var result = DualRuleConverter.ComputeHead(rule);
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
        
        var result = DualRuleConverter.ComputeHead(rule);
        
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
        
        var result = DualRuleConverter.ComputeHead(rule).ToString();
       
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
        
        
        var result = DualRuleConverter.ComputeHead(rule).ToString();
       
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
        
        var result = DualRuleConverter.ComputeHead(rule).ToString();
        
        
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
        
        var result = DualRuleConverter.ComputeHead(rule).ToString();
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


        var result = DualRuleConverter.GetDualRules(rule, ["X,Y"]);

        //not p(X, Y) :- q(X).
        string firstDual = result[0].ToString();
        
        //not p(X, Y) :- not q(X), not t(Y, Y).
        string secondDual = result[1].ToString();
        
        Assert.That(firstDual == "p(X, Y) :- q(X)." && secondDual == "p(X, Y) :- not q(X), not t(Y, Y).");
    }

    [Test]
    public void GetDualWorks()
    {
        string code = """
                      p(0).
                      p(X) :- q(X), not t(X,Y).
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        var dual = DualRuleConverter.GetDualRules(program.Statements);
    }

    [Test]
    public void ForallTreatsSingleBodyVariableCorrectly()
    {
        string code = """
                      q(X) :- not p(X, Y).
                      q?
                      """;

        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        var variables = new HashSet<string>() { "X", "Y" };
        var duals = DualRuleConverter.AddForall(program.Statements[0], variables);
        
        Assert.That(duals.Count == 2 && errorLogger.Errors.Count == 0 &&
                    duals[0].ToString() == "not q(X) :- forall(Y, fa0_q(X, Y))." && 
                    duals[1].ToString() == "fa0_q(X, Y) :- p(X, Y).");
    }

    [Test]
    public void ForallTreatsMultipleBodyVariablesCorrectly()
    {
        string code = """
                      q(X) :- not p(X, Y, Z).
                      q?
                      """;

        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        var variables = new HashSet<string>() { "X", "Y", "Z" };
        var duals = DualRuleConverter.AddForall(program.Statements[0], variables);
        
        Assert.That(duals.Count == 2 && errorLogger.Errors.Count == 0 &&
                    duals[0].ToString() == "not q(X) :- forall(Y, forall(Z, fa0_q(X, Y, Z)))." && 
                    duals[1].ToString() == "fa0_q(X, Y, Z) :- p(X, Y, Z).");
    }
}
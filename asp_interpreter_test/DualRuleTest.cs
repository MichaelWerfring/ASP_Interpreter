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
    }

    [Test]
    public void TransformHeadIgnoresNonVariableTerms()
    {
        
    }
    
    [Test]
    public void TransformHeadIgnoresDistinctVariables()
    {
    }
    
    [Test]
    public void TransformHeadRewritesDuplicateVariables()
    {
    }
    
    [Test]
    public void TransformHeadDoesNotAlterRuleBody()
    {
    }
    
    [Test]
    public void TransformHeadRewritesDuplicateMultipleOccurrences()
    {
    }
    
    [Test]
    public void TransformHeadRewritesDuplicateMultipleOccurrencesOfDifferentVariables()
    {
    }
    
    [Test]
    public void TransformHeadHandlesCompoundTerms()
    {
        string code = """
                      a(X, b(X, Y)) :- c(X, Y).
                      a? 
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        
        var dualRuleConverter = new DualRuleConverter(program);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
        
       
        Assert.That(duals[0].ToString(), Is.EqualTo("a(X, b(rwh0_X, Y)) :- X = rwh0_X, c(X, Y)."));
    }

    [Test]
    public void GetDualRulesForStatementReturnsCorrectDualRules()
    {
        //p(X, Y) :- not q(X), t(Y, Y).
        string code = """
                      p(X, Y) :- not q(X), t(Y, Y).
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        var dualRuleConverter = new DualRuleConverter(program);
        var duals = dualRuleConverter.GetDualRules(program.Statements);

        //not p(X, Y) :- q(X).
        string firstDual = duals[0].ToString();
        
        //not p(X, Y) :- not q(X), not t(Y, Y).
        string secondDual = duals[1].ToString();
        
        Assert.That(firstDual == "not p(X, Y) :- q(X)." && 
                    secondDual == "not p(X, Y) :- not q(X), not t(Y, Y).");
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
        
        var dualRuleConverter = new DualRuleConverter(program);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
        
        Assert.That(duals.Count == 2 && errorLogger.Errors.Count == 0 &&
                    duals[0].ToString() == "not q(X) :- forall(Y, fa0_q(X, Y))." && 
                    duals[1].ToString() == "not fa0_q(X, Y) :- p(X, Y).");
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

        var dualRuleConverter = new DualRuleConverter(program);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
        
        Assert.That(duals.Count == 2 && errorLogger.Errors.Count == 0 &&
                    duals[0].ToString() == "not q(X) :- forall(Y, forall(Z, fa0_q(X, Y, Z)))." && 
                    duals[1].ToString() == "not fa0_q(X, Y, Z) :- p(X, Y, Z).");
    }
    
    [Test]
    public void TestComplexConversion ()
    {
        string code = """
                      p(0).
                      p(X) :- q(X), not t(X, Y).
                      p?
                      """;

        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);

        var dualRuleConverter = new DualRuleConverter(program);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
        
        Assert.That(errorLogger.Errors.Count == 0 && duals.Count == 5 &&
                    duals[0].ToString() == "not p(dis0_) :- not idis0_p(dis0_), not idis1_p(dis0_)." &&
                    duals[1].ToString() == "not idis0_p(rwh0_0) :- rwh0_0 \\= 0." &&
                    duals[2].ToString() == "not idis1_p(X) :- forall(Y, not fa0_idis1_p(X, Y))." &&
                    duals[3].ToString() == "not fa0_idis1_p(X, Y) :- not q(X)." &&
                    duals[4].ToString() == "not fa0_idis1_p(X, Y) :- q(X), t(X, Y).");

    }

    [Test]
    public void GetDualsSkipsEmptyHeads()
    {
        string code = """
                      :- q(X).
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        var dualRuleConverter = new DualRuleConverter(program);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
        
        Assert.That(duals.Count == 0);
    }
    
    [Test]
    public void GetDualsHandlesEmptyBody()
    {
        string code = """
                      p(3).
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(program);
        var dual = dualRuleConverter.GetDualRules(program.Statements);   
        
        Assert.That(dual.Count == 1 && dual[0].ToString() == "not p(rwh0_3) :- rwh0_3 \\= 3.");
    }
}

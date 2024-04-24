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
    private readonly PrefixOptions _prefixes = ASPExtensions.CommonPrefixes;
    
    [Test]
    public void ToDisjunctionHandlesTwoGoals()
    {
        string code = """
                      a(X, Y) :- c(X), not b(X, Y).
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();
        
        Assert.That(dual.Count == 2);
        Assert.Multiple(() =>
        {
            Assert.That(dual[0].ToString() == "not_a(X, Y) :- not c(X).");
            Assert.That(dual[1].ToString() == "not_a(X, Y) :- c(X), b(X, Y).");
        });
    }
    
    [Test]
    public void ToDisjunctionHandlesLiteralsAndBinaryOperations()
    {
        string code = """
                      a(X, Y) :- not b(X), Y = 4.
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();
        
        Assert.That(dual.Count == 2);
        Assert.Multiple(() =>
        {
            Assert.That(dual[0].ToString() == "not_a(X, Y) :- b(X).");
            Assert.That(dual[1].ToString() == "not_a(X, Y) :- not b(X), Y \\= 4.");
        });
    }
    
    [Test]
    public void ToDisjunctionDoesNotAlterClassicalNegation()
    {
        string code = """
                      a(X, Y) :- not -b(X), Y = 4, -c(Y), d(X, Y).
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();
        
        Assert.That(dual.Count == 4);
        Assert.Multiple(() =>
        {
            Assert.That(dual[0].ToString() == "not_a(X, Y) :- -b(X).");
            Assert.That(dual[1].ToString() == "not_a(X, Y) :- not -b(X), Y \\= 4.");
            Assert.That(dual[2].ToString() == "not_a(X, Y) :- not -b(X), Y = 4, not -c(Y).");
            Assert.That(dual[3].ToString() == "not_a(X, Y) :- not -b(X), Y = 4, -c(Y), not d(X, Y).");
        });
    }
    
    [Test]
    public void ToDisjunctionIgnoresEmptyHeads()
    {
        string code = """
                      :- not b(X), c(Y), not d(X, Y).
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();
        
        Assert.That(dual.Count == 1);
        Assert.Multiple(() =>
        {
            Assert.That(dual[0].ToString() == ":- not b(X), c(Y), not d(X, Y).");
        });
    }
    
    [Test]
    public void ToDisjunctionAppliesForallOnPositiveLiteral()
    {
        string code = """
                      a(X) :- not b(X), d(X, Y).
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();
        
        Assert.That(dual.Count == 3);
        Assert.Multiple(() =>
        {
            Assert.That(dual[0].ToString() == "not_a(X) :- forall(Y, fa_a(X, Y)).");
            Assert.That(dual[1].ToString() == "fa_a(X, Y) :- b(X).");
            Assert.That(dual[2].ToString() == "fa_a(X, Y) :- not b(X), not d(X, Y).");
        });
    }
    
    [Test]
    public void ToDisjunctionAppliesForallOnNegativeLiteral()
    {
        string code = """
                      a(X) :- not b(X), not d(X, Y).
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();
        
        Assert.That(dual.Count == 3);
        Assert.Multiple(() =>
        {
            Assert.That(dual[0].ToString() == "not_a(X) :- forall(Y, fa_a(X, Y)).");
            Assert.That(dual[1].ToString() == "fa_a(X, Y) :- b(X).");
            Assert.That(dual[2].ToString() == "fa_a(X, Y) :- not b(X), d(X, Y).");
        });
    }
    
    [Test]
    public void ToDisjunctionAppliesForallOnBinaryOperation()
    {
        string code = """
                      a(X) :- not b(X), Y = 4.
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();
        
        Assert.That(dual.Count == 3);
        Assert.Multiple(() =>
        {
            Assert.That(dual[0].ToString() == "not_a(X) :- forall(Y, fa_a(X, Y)).");
            Assert.That(dual[1].ToString() == "fa_a(X, Y) :- b(X).");
            Assert.That(dual[2].ToString() == "fa_a(X, Y) :- not b(X), Y \\= 4.");
        });
    }
    
    [Test]
    public void ForallDoesNotAlterClassicalNegation()
    {
        string code = """
                      a(X) :- not b(X), -c(Y), not -d(X, Y).
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();
        
        Assert.That(dual.Count == 4);
        Assert.Multiple(() =>
        {
            Assert.That(dual[0].ToString() == "not_a(X) :- forall(Y, fa_a(X, Y)).");
            Assert.That(dual[1].ToString() == "fa_a(X, Y) :- b(X).");
            Assert.That(dual[2].ToString() == "fa_a(X, Y) :- not b(X), not -c(Y).");
            Assert.That(dual[3].ToString() == "fa_a(X, Y) :- not b(X), -c(Y), -d(X, Y).");
        });
    }
    
    [Test]
    public void ForallHandlesMultipleBodyVariables()
    {
        string code = """
                      a(X) :- not b(X), Y = 4, c(X, Y, Z).
                      p?
                      """;

        var program = ASPExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();
        
        Assert.That(dual.Count == 4);
        Assert.Multiple(() =>
        {
            Assert.That(dual[0].ToString() == "not_a(X) :- forall(Y, forall(Z, fa_a(X, Y, Z))).");
            Assert.That(dual[1].ToString() == "fa_a(X, Y, Z) :- b(X).");
            Assert.That(dual[2].ToString() == "fa_a(X, Y, Z) :- not b(X), Y \\= 4.");
            Assert.That(dual[3].ToString() == "fa_a(X, Y, Z) :- not b(X), Y = 4, not c(X, Y, Z).");
        });
    }
}

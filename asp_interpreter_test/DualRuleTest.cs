using System.Xml.Linq;
using asp_interpreter_lib;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util;
using NuGet.Frameworks;
using Is = NUnit.Framework.Is;

namespace asp_interpreter_test;

public class DualRuleTest
{
    private readonly PrefixOptions _prefixes = AspExtensions.CommonPrefixes;
    
    [Test]
    public void ToDisjunctionHandlesTwoGoals()
    {
        string code = """
                      a(X, Y) :- c(X), not b(X, Y).
                      p?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
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

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
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

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
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

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
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

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
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

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
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

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
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

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
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

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
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

    [Test]
    public void ForallSkipsEmptyHeads()
    {
        string code = """
                      :- not b(X), Y = 4, c(X, Y, Z).
                      p?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var dual = dualRuleConverter.AddForall(program.Statements[0]).ToList();
        
        Assert.That(dual.Count == 1);
        Assert.Multiple(() =>
        {
            Assert.That(dual[0].ToString() == ":- not b(X), Y = 4, c(X, Y, Z).");
        });
    }
    
    [Test]
    public void ForallSkipsFacts()
    {
        
        string code = """
                      b(X).
                      p?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var dual = dualRuleConverter.AddForall(program.Statements[0]).ToList();
        
        
        Assert.Multiple(() =>
        {
            Assert.That(dual.Count == 1);
            Assert.That(dual[0].ToString() == "b(X).");
        });
    }
    
    [Test]
    public void ComputeHeadHandlesMultipleVariables()
    {
        string code = """
                      a(X, Y, X, X) :- c(X), b(Y).
                      a?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var statement = dualRuleConverter.ComputeHead(program.Statements[0]);
        
        Assert.That(statement.ToString() == "a(X, Y, V0, V1) :- V1 = X, V0 = X, c(X), b(Y).");
    }

    [Test]
    public void ComputeHeadHandlesIntegerValues()
    {
        string code = """
                      a(X, Y, 12) :- c(X), b(Y).
                      a?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var statement = dualRuleConverter.ComputeHead(program.Statements[0]);
        
        Assert.That(statement.ToString() == "a(X, Y, V0) :- V0 = 12, c(X), b(Y).");
    }

    [Test]
    public void ComputeHeadHandlesAtom()
    {
        string code = """
                      a(b).
                      a?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var statement = dualRuleConverter.ComputeHead(program.Statements[0]);
        
        Assert.That(statement.ToString() == "a(V0) :- V0 = b.");
    }

    [Test]
    public void ComputeHeadHandlesHandlesNegatedTerm()
    {
        string code = """
                      a(-4).
                      a?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var statement = dualRuleConverter.ComputeHead(program.Statements[0]);
        
        Assert.That(statement.ToString() == "a(V0) :- V0 = -4."); 
    }

    [Test]
    public void ComputeHeadHandlesHandlesCompoundTerm()
    {
        string code = """
                      a(b(2, X, X, -3, (4), c)).
                      a?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var statement = dualRuleConverter.ComputeHead(program.Statements[0]);
        
        Assert.That(statement.ToString() == "a(b(V0, X, V1, V2, V3, V4)) :- V4 = c, V3 = 4, V2 = -3, V1 = X, V0 = 2."); 
    }
    
    [Test]
    public void ComputeHeadHandlesHandlesParenthesisedTerm()
    {
        string code = """
                      a(((4))).
                      a?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var statement = dualRuleConverter.ComputeHead(program.Statements[0]);
        
        Assert.That(statement.ToString() == "a(V0) :- V0 = 4."); 
    }
    
    [Test]
    public void ComputeHeadHandlesRecursiveLists()
    {
        string code = """
                      p([X|T]) :- q(X), p(T).
                      p?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var statement = dualRuleConverter.ComputeHead(program.Statements[0]);
        
        Assert.That(statement.ToString() == "p(V0) :- V0 = [X| T], q(X), p(T).");
    }
    
    [Test]
    public void ComputeHeadHandlesConventionalLists()
    {
        string code = """
                      p([X,Y,Z]) :- q(X), r(Y), s(Z).
                      p?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var statement = dualRuleConverter.ComputeHead(program.Statements[0]);
        
        Assert.That(statement.ToString() == "p(V0) :- V0 = [X, Y, Z], q(X), r(Y), s(Z).");
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
        var program = AspExtensions.GetProgram(code, errorLogger);

        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
        
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 7);
            Assert.That(duals[0].ToString() == "not_p(V1) :- not_p1(V1), not_p2(V1).");
            Assert.That(duals[1].ToString() == "not_p1(V0) :- V0 \\= 0.");
            Assert.That(duals[2].ToString() == "not_p2(X) :- forall(Y, fa_p2(X, Y)).");
            Assert.That(duals[3].ToString() == "fa_p2(X, Y) :- not q(X).");
            Assert.That(duals[4].ToString() == "fa_p2(X, Y) :- q(X), t(X, Y).");
            Assert.That(duals[5].ToString() == "not_q(X).");
            Assert.That(duals[6].ToString() == "not_t(X, Y).");
        });
    }
    
    [Test]
    public void ComplexConversionDoesNotAlterClassicalNegation()
    {
        string code = """
                      p(0).
                      p(X) :- -q(X), not -t(X, Y).
                      p?
                      """;


        var errorLogger = new MockErrorLogger();
        var program = AspExtensions.GetProgram(code, errorLogger);

        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
        
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 7);
            Assert.That(duals[0].ToString() == "not_p(V1) :- not_p1(V1), not_p2(V1).");
            Assert.That(duals[1].ToString() == "not_p1(V0) :- V0 \\= 0.");
            Assert.That(duals[2].ToString() == "not_p2(X) :- forall(Y, fa_p2(X, Y)).");
            Assert.That(duals[3].ToString() == "fa_p2(X, Y) :- not -q(X).");
            Assert.That(duals[4].ToString() == "fa_p2(X, Y) :- -q(X), -t(X, Y).");
            Assert.That(duals[5].ToString() == "-not_q(X).");
            Assert.That(duals[6].ToString() == "-not_t(X, Y).");
        });
    }

    [Test]
    public void ComplexConversionHandlesMultipleDisjunctions()
    {
        string code = """
                      penguin(sam).
                      wounded_bird(john).
                      bird(tweety).
                      
                      bird(X) :- penguin(X).
                      bird(X) :- wounded_bird(X).
                      
                      ab(X) :- penguin(X).
                      ab(X) :- wounded_bird(X).
                      
                      flies(X) :- bird(X), not ab(X).
                      flies(sam)?
                      """;
        
        var errorLogger = new MockErrorLogger();
        var program = AspExtensions.GetProgram(code, errorLogger);

        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
        
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 11);
            Assert.That(duals[0].ToString() == "not_penguin(V0) :- V0 \\= sam.");
            Assert.That(duals[1].ToString() == "not_wounded_bird(V0) :- V0 \\= john.");
            Assert.That(duals[2].ToString() == "not_bird(V1) :- not_bird1(V1), not_bird2(V1), not_bird3(V1).");
            Assert.That(duals[3].ToString() == "not_bird1(V0) :- V0 \\= tweety.");
            Assert.That(duals[4].ToString() == "not_bird2(X) :- not penguin(X).");
            Assert.That(duals[5].ToString() == "not_bird3(X) :- not wounded_bird(X).");
            Assert.That(duals[6].ToString() == "not_ab(V1) :- not_ab1(V1), not_ab2(V1).");
            Assert.That(duals[7].ToString() == "not_ab1(X) :- not penguin(X).");
            Assert.That(duals[8].ToString() == "not_ab2(X) :- not wounded_bird(X).");
            Assert.That(duals[9].ToString() == "not_flies(X) :- not bird(X).");
            Assert.That(duals[10].ToString()== "not_flies(X) :- bird(X), ab(X).");
        });
    }

    [Test]
    public void GeneratesDualsForClassicalNegationSeparately()
    {
        string code = """
                      p(X) :- not q(X), r(X).
                      -p(X) :- s(X), not t(X).
                      p(4)?
                      """;
        
        var errorLogger = new MockErrorLogger();
        var program = AspExtensions.GetProgram(code, errorLogger);

        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
        
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 8);
            Assert.That(duals[0].ToString() == "not_p(X) :- q(X).");
            Assert.That(duals[1].ToString() == "not_p(X) :- not q(X), not r(X).");
            Assert.That(duals[2].ToString() == "-not_p(X) :- not s(X).");
            Assert.That(duals[3].ToString() == "-not_p(X) :- s(X), t(X).");
            Assert.That(duals[4].ToString() == "not_q(X).");
            Assert.That(duals[5].ToString() == "not_r(X).");
            Assert.That(duals[6].ToString() == "not_s(X).");
            Assert.That(duals[7].ToString() == "not_t(X).");
        });
    }
    
    [Test]
    public void ComplexConversionHandlesClassicalNegationInHead()
    {
        string code = """
                      penguin(sam).
                      wounded_bird(john).
                      bird(tweety).
                      
                      bird(X) :- penguin(X).
                      bird(X) :- wounded_bird(X).
                      
                      ab(X) :- penguin(X).
                      ab(X) :- wounded_bird(X).
                      
                      flies(X) :- bird(X), not ab(X).
                      
                      -flies(X) :- ab(X).
                      -flies(X) :- -bird(X).
                      
                      -wounded_bird(X) :- not wounded_bird(X).
                      -penguin(X) :- not penguin(X).
                      -ab(X) :- not ab(X).
                      -bird(X) :- not bird(X).
                      
                      flies(sam)?
                      """;
        
        var errorLogger = new MockErrorLogger();
        var program = AspExtensions.GetProgram(code, errorLogger);

        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var duals = dualRuleConverter.GetDualRules(program.Statements);

        //Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 18);
            Assert.That(duals[0].ToString() == "not_penguin(V0) :- V0 \\= sam.");
            Assert.That(duals[1].ToString() == "not_wounded_bird(V0) :- V0 \\= john.");
            Assert.That(duals[2].ToString() == "not_bird(V1) :- not_bird1(V1), not_bird2(V1), not_bird3(V1).");
            Assert.That(duals[3].ToString() == "not_bird1(V0) :- V0 \\= tweety.");
            Assert.That(duals[4].ToString() == "not_bird2(X) :- not penguin(X).");
            Assert.That(duals[5].ToString() == "not_bird3(X) :- not wounded_bird(X).");
            Assert.That(duals[6].ToString() == "not_ab(V1) :- not_ab1(V1), not_ab2(V1).");
            Assert.That(duals[7].ToString() == "not_ab1(X) :- not penguin(X).");
            Assert.That(duals[8].ToString() == "not_ab2(X) :- not wounded_bird(X).");
            Assert.That(duals[9].ToString() == "not_flies(X) :- not bird(X).");
            Assert.That(duals[10].ToString()== "not_flies(X) :- bird(X), ab(X).");
            Assert.That(duals[11].ToString()== "-not_flies(V1) :- -not_flies1(V1), -not_flies2(V1).");
            Assert.That(duals[12].ToString()== "-not_flies1(X) :- not ab(X).");
            Assert.That(duals[13].ToString()== "-not_flies2(X) :- not -bird(X).");
            Assert.That(duals[14].ToString()== "-not_wounded_bird(X) :- wounded_bird(X).");
            Assert.That(duals[15].ToString()== "-not_penguin(X) :- penguin(X).");
            Assert.That(duals[16].ToString()== "-not_ab(X) :- ab(X).");
            Assert.That(duals[17].ToString()== "-not_bird(X) :- bird(X).");
        });
    }
    
    [Test]
    public void TreatsRecursiveListInHeadWithForall()
    {
        string code = """
                      p([X|T]) :- q(X), p(T).
                      p?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
        
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 5);
            Assert.That(duals[0].ToString() == "not_p(V0) :- forall(X, forall(T, fa_p(V0, X, T))).");
            Assert.That(duals[1].ToString() == "fa_p(V0, X, T) :- V0 \\= [X| T].");
            Assert.That(duals[2].ToString() == "fa_p(V0, X, T) :- V0 = [X| T], not q(X).");
            Assert.That(duals[3].ToString() == "fa_p(V0, X, T) :- V0 = [X| T], q(X), not p(T).");
            Assert.That(duals[4].ToString() == "not_q(X).");
        });
    }
    
    [Test]
    public void TreatsConventionalListInHeadWithForall()
    { 
        string code = """
                      p([X, Y, Z]) :- q(X), r(Y), s(Z).
                      p?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
    
        //Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 8);
            Assert.That(duals[0].ToString() == "not_p(V0) :- forall(X, forall(Y, forall(Z, fa_p(V0, X, Y, Z)))).");
            Assert.That(duals[1].ToString() == "fa_p(V0, X, Y, Z) :- V0 \\= [X, Y, Z].");
            Assert.That(duals[2].ToString() == "fa_p(V0, X, Y, Z) :- V0 = [X, Y, Z], not q(X).");
            Assert.That(duals[3].ToString() == "fa_p(V0, X, Y, Z) :- V0 = [X, Y, Z], q(X), not r(Y).");
            Assert.That(duals[4].ToString() == "fa_p(V0, X, Y, Z) :- V0 = [X, Y, Z], q(X), r(Y), not s(Z).");
            Assert.That(duals[5].ToString() == "not_q(X).");
            Assert.That(duals[6].ToString() == "not_r(Y).");
            Assert.That(duals[7].ToString() == "not_s(Z).");
        });
    }
    
    [Test]
    public void DualRuleConversionHandlesAtoms()
    {
        string code = """
                      -p.
                      p :- -p.
                      p?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());
        
        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var duals = dualRuleConverter.GetDualRules(program.Statements);
        
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 2);
            Assert.That(duals[0].ToString() == "-not_p.");
            Assert.That(duals[1].ToString() == "not_p :- not -p.");
        });
    }

    [Test]
    public void DualRuleConverterPutsGoalsOnlyInBodyIntoFacts()
    {
        string code = """
                      p :- s.
                      p :- not q.
                      q :- not p.
                      r :- p.
                      p?
                      """;

        var program = AspExtensions.GetProgram(code, new MockErrorLogger());

        var dualRuleConverter = new DualRuleConverter(_prefixes);
        var duals = dualRuleConverter.GetDualRules(program.Statements);

        //Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 6);
            Assert.That(duals[0].ToString() == "not_p :- not_p1, not_p2.");
            Assert.That(duals[1].ToString() == "not_p1 :- not s.");
            Assert.That(duals[2].ToString() == "not_p2 :- q.");
            Assert.That(duals[3].ToString() == "not_q :- p.");
            Assert.That(duals[4].ToString() == "not_r :- not p.");
            Assert.That(duals[5].ToString() == "not_s.");
        });


    }
}
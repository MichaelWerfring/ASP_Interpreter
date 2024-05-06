using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_test.DualRules;

public class DualRuleTest
{
    private readonly PrefixOptions _prefixes = AspExtensions.CommonPrefixes;

    private readonly ILogger _logger = new TestingLogger(LogLevel.Error);

    [Test]
    public void ForallSkipsEmptyHeads()
    {
        string code = """
                      :- not b(X), Y = 4, c(X, Y, Z).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, _logger);
        var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

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
                      ?- b(X).
                      """;

        var program = AspExtensions.GetProgram(code, _logger);
        var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

        var dual = dualRuleConverter.AddForall(program.Statements[0]).ToList();


        Assert.Multiple(() =>
        {
            Assert.That(dual.Count == 1);
            Assert.That(dual[0].ToString() == "b(X).");
        });
    }

    [Test]
    public void TestComplexConversion()
    {
        string code = """
                      p(0).
                      p(X) :- q(X), not t(X, Y).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, _logger);
        var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

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
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, _logger);
        var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

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
    public void GeneratesDualsForClassicalNegationSeparately()
    {
        string code = """
                      p(X) :- not q(X), r(X).
                      -p(X) :- s(X), not t(X).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, _logger);
        var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

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
    public void TreatsRecursiveListInHeadWithForall()
    {
        string code = """
                      p([X|T]) :- q(X), p(T).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, _logger);
        var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

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
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, _logger);

        var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);
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
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, _logger);
        var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

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
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, _logger);
        var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

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
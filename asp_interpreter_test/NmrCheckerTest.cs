using asp_interpreter_lib;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.NMRCheck;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_test;

public class NmrCheckerTest
{
    private readonly PrefixOptions _prefixes = AspExtensions.CommonPrefixes;

    private readonly ILogger _logger = new TestingLogger(LogLevel.Error);

    [Test]
    public void NmrCheckHandlesBasicProgram()
    {
        string code = """
                      p(X) :- q(X), not p(X).
                      ?- p(X).
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);    

        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);

        Assert.Multiple(() =>
        {
            Assert.That(subCheckRules.Count, Is.EqualTo(4));
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check :- forall(X, not(_chk_1_(X)))."));
            Assert.That(subCheckRules[1].ToString(), Is.EqualTo("not(_chk_1_(V1)) :- not(_chk_1_1(V1))."));
            Assert.That(subCheckRules[2].ToString(), Is.EqualTo("not(_chk_1_1(X)) :- not(q(X))."));
            Assert.That(subCheckRules[3].ToString(), Is.EqualTo("not(_chk_1_1(X)) :- q(X), p(X)."));
        });


    }

    [Test]
    public void NmrCheckHandlesEmptyRuleHeads()
    {
        string code = """
                      :- not r(X).
                      ?- p(X).
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);
        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);

        Assert.Multiple(() =>
        {
            Assert.That(subCheckRules.Count, Is.EqualTo(4));
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check :- not(_chk_1_)."));
            Assert.That(subCheckRules[1].ToString(), Is.EqualTo("not(_chk_1_) :- not(_chk_1_1)."));
            Assert.That(subCheckRules[2].ToString(), Is.EqualTo("not(_chk_1_1) :- forall(X, not(_fa__chk_1_1(X)))."));
            Assert.That(subCheckRules[3].ToString(), Is.EqualTo("not(_fa__chk_1_1(X)) :- r(X)."));
        });


    }

    [Test]
    public void NmrCheckHandlesCompoundProgram()
    {
        string code = """
                      p(X) :- q(X), not p(X).
                      :- not r(X).
                      ?- p(X).
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);
        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);

        //verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(subCheckRules.Count, Is.EqualTo(7));
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check :- forall(X, not(_chk_1_(X))), not(_chk_2_)."));
            Assert.That(subCheckRules[1].ToString(), Is.EqualTo("not(_chk_1_(V1)) :- not(_chk_1_1(V1))."));
            Assert.That(subCheckRules[2].ToString(), Is.EqualTo("not(_chk_1_1(X)) :- not(q(X))."));
            Assert.That(subCheckRules[3].ToString(), Is.EqualTo("not(_chk_1_1(X)) :- q(X), p(X)."));
            Assert.That(subCheckRules[4].ToString(), Is.EqualTo("not(_chk_2_) :- not(_chk_2_1)."));
            Assert.That(subCheckRules[5].ToString(), Is.EqualTo("not(_chk_2_1) :- forall(X, not(_fa__chk_2_1(X)))."));
            Assert.That(subCheckRules[6].ToString(), Is.EqualTo("not(_fa__chk_2_1(X)) :- r(X)."));
        });
    }
    
    [Test]
    public void NmrCheckHandlesCompoundProgramWithAtom()
    {
        string code = """
                      :- not s(1, X).
                      p(X):- q(X), not p(X).
                      ?- p(X).
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);
        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);

        //verifed with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(subCheckRules.Count, Is.EqualTo(7));
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check :- not(_chk_1_), forall(X, not(_chk_2_(X)))."));
            Assert.That(subCheckRules[1].ToString(), Is.EqualTo("not(_chk_1_) :- not(_chk_1_1)."));
            Assert.That(subCheckRules[2].ToString(), Is.EqualTo("not(_chk_1_1) :- forall(X, not(_fa__chk_1_1(X)))."));
            Assert.That(subCheckRules[3].ToString(), Is.EqualTo("not(_fa__chk_1_1(X)) :- s(1, X)."));
            Assert.That(subCheckRules[4].ToString(), Is.EqualTo("not(_chk_2_(V1)) :- not(_chk_2_1(V1))."));
            Assert.That(subCheckRules[5].ToString(), Is.EqualTo("not(_chk_2_1(X)) :- not(q(X))."));
            Assert.That(subCheckRules[6].ToString(), Is.EqualTo("not(_chk_2_1(X)) :- q(X), p(X)."));
        });
    }

    [Test]
    public void NmrCheckAddsHeadOfRuleToBodyIfNecessary()
    {
        string code = """
                      p(X) :- q(X, Y), not p(Y).
                      ?- p(X).
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);
        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);

        //verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(subCheckRules.Count, Is.EqualTo(6));
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check :- forall(X, not(_chk_1_(X)))."));
            Assert.That(subCheckRules[1].ToString(), Is.EqualTo("not(_chk_1_(V1)) :- not(_chk_1_1(V1))."));
            Assert.That(subCheckRules[2].ToString(), Is.EqualTo("not(_chk_1_1(X)) :- forall(Y, not(_fa__chk_1_1(X, Y)))."));
            Assert.That(subCheckRules[3].ToString(), Is.EqualTo("not(_fa__chk_1_1(X, Y)) :- not(q(X, Y))."));
            Assert.That(subCheckRules[4].ToString(), Is.EqualTo("not(_fa__chk_1_1(X, Y)) :- q(X, Y), p(Y)."));
            Assert.That(subCheckRules[5].ToString(), Is.EqualTo("not(_fa__chk_1_1(X, Y)) :- q(X, Y), not(p(Y)), p(X)."));
        });
    }

    [Test]
    public void AlsoAddsCheckForEmtpyStatements()
    {
        var checker = new NmrChecker(_prefixes, _logger);
        var subCheckRules = checker.GetSubCheckRules([], false);

        //verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(subCheckRules.Count, Is.EqualTo(1));
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check."));
        });
    }

    [Test]
    public void NMRCheckerDoesNotNestForallMoreThanOnce()
    {
        string code = """
                      other(U, V) :-
                          vertex(U), vertex(V), vertex(W),
                          edge(U, W), V \= W, chosen(U, W).
                      chosen(U, V) :-
                          edge(U, V), not other(U, V).

                      :- chosen(U, W), chosen(V, W), U \= V.

                      ?- chosen(X, Y).
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);
        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);

        //verified with s(CASP)
        Assert.Multiple(() =>
        {
            //Just check the nmr_rule is important now
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo(
                "_nmr_check :- forall(U, forall(V, not(_chk_1_(U, V)))), forall(U, forall(V, not(_chk_2_(U, V)))), not(_chk_3_)."));
        });
    }

    [Test]
    public void NMRCheckerTreatsDisjunctionsAsSeparateRules()
    {
        string code = """
                      p :- not q.
                      q :- not r.
                      r :- not p.
                      q :- not r.
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);
        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);

        //verified with s(CASP)
        Assert.Multiple(() =>
        {
            //Just check the nmr_rule is important now
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check :- not(_chk_1_), not(_chk_2_), not(_chk_3_), not(_chk_4_)."));
            Assert.That(subCheckRules[1].ToString(), Is.EqualTo("not(_chk_1_) :- not(_chk_1_1)."));
            Assert.That(subCheckRules[2].ToString(), Is.EqualTo("not(_chk_1_1) :- q."));
            Assert.That(subCheckRules[3].ToString(), Is.EqualTo("not(_chk_1_1) :- not(q), p."));
            Assert.That(subCheckRules[4].ToString(), Is.EqualTo("not(_chk_2_) :- not(_chk_2_1)."));
            Assert.That(subCheckRules[5].ToString(), Is.EqualTo("not(_chk_2_1) :- r."));
            Assert.That(subCheckRules[6].ToString(), Is.EqualTo("not(_chk_2_1) :- not(r), q."));
            Assert.That(subCheckRules[7].ToString(), Is.EqualTo("not(_chk_3_) :- not(_chk_3_1)."));
            Assert.That(subCheckRules[8].ToString(), Is.EqualTo("not(_chk_3_1) :- p."));
            Assert.That(subCheckRules[9].ToString(), Is.EqualTo("not(_chk_3_1) :- not(p), r."));
            Assert.That(subCheckRules[10].ToString(), Is.EqualTo("not(_chk_4_) :- not(_chk_4_1)."));
            Assert.That(subCheckRules[11].ToString(), Is.EqualTo("not(_chk_4_1) :- r."));
            Assert.That(subCheckRules[12].ToString(), Is.EqualTo("not(_chk_4_1) :- not(r), q."));
        });
    }
}
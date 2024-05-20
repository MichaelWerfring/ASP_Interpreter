﻿using asp_interpreter_lib;
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
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check :- forall(X, not(_chk_p(X)))."));
            Assert.That(subCheckRules[1].ToString(), Is.EqualTo("not(_chk_p(V1)) :- not(_chk_p1(V1))."));
            Assert.That(subCheckRules[2].ToString(), Is.EqualTo("not(_chk_p1(X)) :- not(q(X))."));
            Assert.That(subCheckRules[3].ToString(), Is.EqualTo("not(_chk_p1(X)) :- q(X), p(X)."));
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
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check :- not(_chk_eh0)."));
            Assert.That(subCheckRules[1].ToString(), Is.EqualTo("not(_chk_eh0) :- not(_chk_eh01)."));
            Assert.That(subCheckRules[2].ToString(), Is.EqualTo("not(_chk_eh01) :- forall(X, not(_chk_fa_eh01(X)))."));
            Assert.That(subCheckRules[3].ToString(), Is.EqualTo("not(_chk_fa_eh01(X)) :- r(X)."));
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
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check :- forall(X, not(_chk_p(X))), not(_chk_eh0)."));
            Assert.That(subCheckRules[1].ToString(), Is.EqualTo("not(_chk_p(V1)) :- not(_chk_p1(V1))."));
            Assert.That(subCheckRules[2].ToString(), Is.EqualTo("not(_chk_p1(X)) :- not(q(X))."));
            Assert.That(subCheckRules[3].ToString(), Is.EqualTo("not(_chk_p1(X)) :- q(X), p(X)."));
            Assert.That(subCheckRules[4].ToString(), Is.EqualTo("not(_chk_eh0) :- not(_chk_eh01)."));
            Assert.That(subCheckRules[5].ToString(), Is.EqualTo("not(_chk_eh01) :- forall(X, not(_chk_fa_eh01(X)))."));
            Assert.That(subCheckRules[6].ToString(), Is.EqualTo("not(_chk_fa_eh01(X)) :- r(X)."));
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
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check :- not(_chk_eh0), forall(X, not(_chk_p(X)))."));
            Assert.That(subCheckRules[1].ToString(), Is.EqualTo("not(_chk_eh0) :- not(_chk_eh01)."));
            Assert.That(subCheckRules[2].ToString(), Is.EqualTo("not(_chk_eh01) :- forall(X, not(_chk_fa_eh01(X)))."));
            Assert.That(subCheckRules[3].ToString(), Is.EqualTo("not(_chk_fa_eh01(X)) :- s(1, X)."));
            Assert.That(subCheckRules[4].ToString(), Is.EqualTo("not(_chk_p(V1)) :- not(_chk_p1(V1))."));
            Assert.That(subCheckRules[5].ToString(), Is.EqualTo("not(_chk_p1(X)) :- not(q(X))."));
            Assert.That(subCheckRules[6].ToString(), Is.EqualTo("not(_chk_p1(X)) :- q(X), p(X)."));
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
            Assert.That(subCheckRules[0].ToString(), Is.EqualTo("_nmr_check :- forall(X, not(_chk_p(X)))."));
            Assert.That(subCheckRules[1].ToString(), Is.EqualTo("not(_chk_p(V1)) :- not(_chk_p1(V1))."));
            Assert.That(subCheckRules[2].ToString(), Is.EqualTo("not(_chk_p1(X)) :- forall(Y, not(_chk_fa_p1(X, Y)))."));
            Assert.That(subCheckRules[3].ToString(), Is.EqualTo("not(_chk_fa_p1(X, Y)) :- not(q(X, Y))."));
            Assert.That(subCheckRules[4].ToString(), Is.EqualTo("not(_chk_fa_p1(X, Y)) :- q(X, Y), p(Y)."));
            Assert.That(subCheckRules[5].ToString(), Is.EqualTo("not(_chk_fa_p1(X, Y)) :- q(X, Y), not(p(Y)), p(X)."));
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
                "_nmr_check :- forall(U, forall(V, not(_chk_other(U, V)))), forall(U, forall(V, not(_chk_chosen(U, V)))), not(_chk_eh0)."));
        });
    }
}
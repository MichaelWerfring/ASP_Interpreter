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
                      p(a)?
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);    

        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);

        Assert.That(subCheckRules.Count == 4 &&
                    subCheckRules[0].ToString() == "nmr_check :- forall(X, not chk_p(X))." &&
                    subCheckRules[1].ToString() == "not chk_p(X) :- not q(X)." &&
                    subCheckRules[2].ToString() == "not chk_p(X) :- q(X), p(X)." &&
                    subCheckRules[3].ToString() == "not chk_q(X).");
    }
    
    [Test]
    public void NmrCheckHandlesEmptyRuleHeads()
    {
        string code = """
                      :- not r(X).
                      p(a)?
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);
        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);
        
        Assert.That(subCheckRules.Count == 4 &&
                    subCheckRules[0].ToString() == "nmr_check :- not chk_eh0." &&
                    subCheckRules[1].ToString() == "not chk_eh0 :- forall(X, not chk_fa_eh0(X))." &&
                    subCheckRules[2].ToString() == "not chk_fa_eh0(X) :- r(X)." &&
                    subCheckRules[3].ToString() == "not chk_r(X).");

    }
    
    [Test]
    public void NmrCheckHandlesCompoundProgram()
    {
        string code = """
                      p(X) :- q(X), not p(X).
                      :- not r(X).
                      p(a)?
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);
        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);
        
        Assert.That(subCheckRules.Count == 7 &&
                    subCheckRules[0].ToString() == "nmr_check :- forall(X, not chk_p(X)), not chk_eh0." &&
                    subCheckRules[1].ToString() == "not chk_p(X) :- not q(X)." &&
                    subCheckRules[2].ToString() == "not chk_p(X) :- q(X), p(X)." &&
                    subCheckRules[3].ToString() == "not chk_eh0 :- forall(X, not chk_fa_eh0(X))." &&
                    subCheckRules[4].ToString() == "not chk_fa_eh0(X) :- r(X)."&&
                    subCheckRules[5].ToString() == "not chk_q(X)." &&
                    subCheckRules[6].ToString() == "not chk_r(X).");
    }
    
    [Test]
    public void NmrCheckHandlesCompoundProgramWithAtom()
    {
        string code = """
                      :- not s(1, X).
                      p(X):- q(X), not p(X).
                      p(a)?
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);
        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);
        
        Assert.That(subCheckRules.Count ==7 &&
                    subCheckRules[0].ToString() == "nmr_check :- not chk_eh0, forall(X, not chk_p(X))." &&
                    subCheckRules[1].ToString() == "not chk_eh0 :- forall(X, not chk_fa_eh0(X))." &&
                    subCheckRules[2].ToString() == "not chk_fa_eh0(X) :- s(1, X)." &&
                    subCheckRules[3].ToString() == "not chk_p(X) :- not q(X)." &&
                    subCheckRules[4].ToString() == "not chk_p(X) :- q(X), p(X)." &&
                    subCheckRules[5].ToString() == "not chk_s(1, X)." &&
                    subCheckRules[6].ToString() == "not chk_q(X).");
    }

    [Test]
    public void NmrCheckAddsHeadOfRuleToBodyIfNecessary()
    {
        string code = """
                      p(X) :- q(X, Y), not p(Y).
                      p(a)?
                      """;
        var program = AspExtensions.GetProgram(code, _logger);
        var checker = new NmrChecker(_prefixes, _logger);
        var subCheckRules = checker.GetSubCheckRules(program.Statements, false);
        
        Assert.That(subCheckRules.Count == 6 &&
                    subCheckRules[0].ToString() == "nmr_check :- forall(X, not chk_p(X))." &&
                    subCheckRules[1].ToString() == "not chk_p(X) :- forall(Y, not chk_fa_p(X, Y))." &&
                    subCheckRules[2].ToString() == "not chk_fa_p(X, Y) :- not q(X, Y)." &&
                    subCheckRules[3].ToString() == "not chk_fa_p(X, Y) :- q(X, Y), p(Y)." &&
                    subCheckRules[4].ToString() == "not chk_fa_p(X, Y) :- q(X, Y), not p(Y), p(X)." &&
                    subCheckRules[5].ToString() == "not chk_q(X, Y).");

    }
}
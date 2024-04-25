using asp_interpreter_lib;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.NMRCheck;

namespace asp_interpreter_test;

public class NmrCheckerTest
{
    private readonly PrefixOptions _prefixes = ASPExtensions.CommonPrefixes;
    
    [Test]
    public void NmrCheckHandlesBasicProgram()
    {
        string code = """
                      p(X) :- q(X), not p(X).
                      p(a)?
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        var checker = new NmrChecker(_prefixes);    
        var subCheckRules = checker.GetSubCheckRules(program.Statements);
        
        Assert.That(subCheckRules.Count == 3 &&
                    subCheckRules[0].ToString() == "nmr_check :- forall(X, chk_not_p(X))." &&
                    subCheckRules[1].ToString() == "chk_not_p(X) :- not q(X)." &&
                    subCheckRules[2].ToString() == "chk_not_p(X) :- q(X), p(X).");
    }
    
    [Test]
    public void NmrCheckHandlesEmptyRuleHeads()
    {
        string code = """
                      :- not r(X).
                      p(a)?
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        var checker = new NmrChecker(_prefixes);    
        var subCheckRules = checker.GetSubCheckRules(program.Statements);
        
        Assert.That(subCheckRules.Count == 3 &&
                    subCheckRules[0].ToString() == "nmr_check :- chk_not_eh0." &&
                    subCheckRules[1].ToString() == "chk_not_eh0 :- forall(X, fa_eh0(X))." &&
                    subCheckRules[2].ToString() == "fa_eh0(X) :- r(X).");
    }
    
    [Test]
    public void NmrCheckHandlesCompoundProgram()
    {
        string code = """
                      p(X) :- q(X), not p(X).
                      :- not r(X).
                      p(a)?
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        var checker = new NmrChecker(_prefixes);    
        var subCheckRules = checker.GetSubCheckRules(program.Statements);
        
        Assert.That(subCheckRules.Count == 5 &&
                    subCheckRules[0].ToString() == "nmr_check :- forall(X, chk_not_p(X)), chk_eh0." &&
                    subCheckRules[1].ToString() == "chk_not_p(X) :- not q(X)." &&
                    subCheckRules[2].ToString() == "chk_not_p(X) :- q(X), p(X)." &&
                    subCheckRules[3].ToString() == "chk_eh0 :- forall(X, fa0_eh0(X))." &&
                    subCheckRules[4].ToString() == "fa0_eh0(X) :- r(X).");
    }
    
    [Test]
    public void NmrCheckHandlesCompoundProgramWithAtom()
    {
        string code = """
                      :- not s(1, X).
                      p(X):- q(X), not p(X).
                      p(a)?
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        var checker = new NmrChecker(_prefixes);    
        var subCheckRules = checker.GetSubCheckRules(program.Statements);
        
        Assert.That(subCheckRules.Count == 5 &&
                    subCheckRules[0].ToString() == "nmr_check :- chk_eh0, forall(X, chk_not_p(X))." &&
                    subCheckRules[1].ToString() == "chk_eh0 :- forall(X, fa0_eh0(X))." &&
                    subCheckRules[2].ToString() == "fa0_eh0(X) :- s(1, X)." &&
                    subCheckRules[3].ToString() == "chk_not_p(X) :- not q(X)." &&
                    subCheckRules[4].ToString() == "chk_not_p(X) :- q(X), p(X).");
    }

    [Test]
    public void NmrCheckAddsHeadOfRuleToBodyIfNecessary()
    {
        string code = """
                      p(X) :- q(X, Y), not p(Y).
                      p(a)?
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        var checker = new NmrChecker(_prefixes);    
        var subCheckRules = checker.GetSubCheckRules(program.Statements);
        
        Assert.That(subCheckRules.Count == 5 &&
                    subCheckRules[0].ToString() == "nmr_check :- forall(X, chk_not_p(X))." &&
                    subCheckRules[1].ToString() == "chk_not_p(X) :- forall(Y, fa_p(X, Y))." &&
                    subCheckRules[2].ToString() == "fa_p(X, Y) :- not q(X, Y)." &&
                    subCheckRules[3].ToString() == "fa_p(X, Y) :- q(X, Y), p(Y)." &&
                    subCheckRules[4].ToString() == "fa_p(X, Y) :- q(X, Y), not p(Y), p(X).");

    }
}
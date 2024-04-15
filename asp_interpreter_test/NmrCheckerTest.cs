using asp_interpreter_lib;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.NMRCheck;

namespace asp_interpreter_test;

public class NmrCheckerTest
{
    [Test]
    public void NmrCheckerComputesSubcheck()
    {
        string code = """
                      p :- r, not p.
                      p :- not q, not p.
                      p :- q, r, not p.
                      p?
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        
        var subCheckRules = NmrChecker.GetSubCheckRules(program.Statements);
        
        Assert.That(subCheckRules.Count == 8 &&
                    subCheckRules[0].ToString() == "nmr_check :- chk0_p, chk1_p, chk2_p." &&
                    subCheckRules[1].ToString() == "chk0_p :- not r." &&
                    subCheckRules[2].ToString() == "chk0_p :- p." &&
                    subCheckRules[3].ToString() == "chk1_p :- q." &&
                    subCheckRules[4].ToString() == "chk1_p :- p." &&
                    subCheckRules[5].ToString() == "chk2_p :- not q." &&
                    subCheckRules[6].ToString() == "chk2_p :- not r." &&
                    subCheckRules[7].ToString() == "chk2_p :- p.");
    }
    
    [Test]
    public void NmrCheckerComputesSubcheckWithEmptyHead()
    {
        string code = """
                      :- r, not p.
                      :- not q, not p.
                      :- q, r, not p.
                      p?
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        
        var subCheckRules = NmrChecker.GetSubCheckRules(program.Statements);
        
        Assert.That(subCheckRules.Count == 8 &&
                    subCheckRules[0].ToString() == "nmr_check :- chk0_, chk1_, chk2_." &&
                    subCheckRules[1].ToString() == "chk0_ :- not r." &&
                    subCheckRules[2].ToString() == "chk0_ :- p." &&
                    subCheckRules[3].ToString() == "chk1_ :- q." &&
                    subCheckRules[4].ToString() == "chk1_ :- p." &&
                    subCheckRules[5].ToString() == "chk2_ :- not q." &&
                    subCheckRules[6].ToString() == "chk2_ :- not r." &&
                    subCheckRules[7].ToString() == "chk2_ :- p.");
    }
    
    [Test]
    public void NmrCheckerComputesSubcheckWithVariables()
    {
        string code = """
                      p(X) :- r(X), not p(X).
                      p(X) :- not q(X), not p(X).
                      p(X) :- q(X), r(X), not p(X).
                      p(a)?
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        
        var subCheckRules = NmrChecker.GetSubCheckRules(program.Statements);
        
        Assert.That(subCheckRules.Count == 8 &&
                    subCheckRules[0].ToString() == "nmr_check :- chk0_p(X), chk1_p(X), chk2_p(X)." &&
                    subCheckRules[1].ToString() == "chk0_p(X) :- not r(X)." &&
                    subCheckRules[2].ToString() == "chk0_p(X) :- p(X)." &&
                    subCheckRules[3].ToString() == "chk1_p(X) :- q(X)." &&
                    subCheckRules[4].ToString() == "chk1_p(X) :- p(X)." &&
                    subCheckRules[5].ToString() == "chk2_p(X) :- not q(X)." &&
                    subCheckRules[6].ToString() == "chk2_p(X) :- not r(X)." &&
                    subCheckRules[7].ToString() == "chk2_p(X) :- p(X).");
    }

    [Test]
    public void NmrCheckComputesSubcheckWithDifferentHeads()
    {
        string code = """
                      p(X) :- r(X), not p(X).
                      p :- not q, not p.
                      :- q.
                      p(a)?
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        
        var subCheckRules = NmrChecker.GetSubCheckRules(program.Statements);
        
        Assert.That(subCheckRules.Count == 6 &&
                    subCheckRules[0].ToString() == "nmr_check :- chk0_p(X), chk1_p, chk0_." &&
                    subCheckRules[1].ToString() == "chk0_p(X) :- not r(X)." &&
                    subCheckRules[2].ToString() == "chk0_p(X) :- p(X)." &&
                    subCheckRules[3].ToString() == "chk1_p :- q." &&
                    subCheckRules[4].ToString() == "chk1_p :- p." &&
                    subCheckRules[5].ToString() == "chk0_ :- not q.");
    }

    [Test]
    public void NmrCheckWorksWithDualRules()
    {
        string code = """
                      p(X) :- q(X), not p(X).
                      p(a)?
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        
        var subCheckRules = NmrChecker.GetSubCheckRules(program.Statements);
        
        Assert.That(subCheckRules.Count == 3 &&
                    subCheckRules[0].ToString() == "nmr_check :- forall(X, chk0_p(X))." &&
                    subCheckRules[1].ToString() == "chk0_p(X) :- not q(X)." &&
                    subCheckRules[2].ToString() == "chk0_p(X) :- q(X), p(X).");
    }

    [Test]
    public void NmrCheckHandlesComplexRulesAndForall()
    {
        string code = """
                      p(X) :- q(X, Y), not p(Y).

                      p(a)?
                      """;
        var errorLogger = new MockErrorLogger();
        var program = ASPExtensions.GetProgram(code, errorLogger);
        
        var subCheckRules = NmrChecker.GetSubCheckRules(program.Statements);
    }
}
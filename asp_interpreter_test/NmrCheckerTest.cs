using asp_interpreter_lib;
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
        
        Assert.That(subCheckRules.Count == 7 &&
                    subCheckRules[0].ToString() == "chk0_p :- not r." &&
                    subCheckRules[1].ToString() == "chk0_p :- p." &&
                    subCheckRules[2].ToString() == "chk1_p :- q." &&
                    subCheckRules[3].ToString() == "chk1_p :- p." &&
                    subCheckRules[4].ToString() == "chk2_p :- not q." &&
                    subCheckRules[5].ToString() == "chk2_p :- not r." &&
                    subCheckRules[6].ToString() == "chk2_p :- p.");
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
        
        Assert.That(subCheckRules.Count == 7 &&
                    subCheckRules[0].ToString() == "chk0_empty_head :- not r." &&
                    subCheckRules[1].ToString() == "chk0_empty_head :- p." &&
                    subCheckRules[2].ToString() == "chk1_empty_head :- q." &&
                    subCheckRules[3].ToString() == "chk1_empty_head :- p." &&
                    subCheckRules[4].ToString() == "chk2_empty_head :- not q." &&
                    subCheckRules[5].ToString() == "chk2_empty_head :- not r." &&
                    subCheckRules[6].ToString() == "chk2_empty_head :- p.");
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
        
        Assert.That(subCheckRules.Count == 7 &&
                    subCheckRules[0].ToString() == "chk0_p(X) :- not r(X)." &&
                    subCheckRules[1].ToString() == "chk0_p(X) :- p(X)." &&
                    subCheckRules[2].ToString() == "chk1_p(X) :- q(X)." &&
                    subCheckRules[3].ToString() == "chk1_p(X) :- p(X)." &&
                    subCheckRules[4].ToString() == "chk2_p(X) :- not q(X)." &&
                    subCheckRules[5].ToString() == "chk2_p(X) :- not r(X)." &&
                    subCheckRules[6].ToString() == "chk2_p(X) :- p(X).");
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
        
        Assert.That(subCheckRules.Count == 5 &&
                    subCheckRules[0].ToString() == "chk0_p(X) :- not r(X)." &&
                    subCheckRules[1].ToString() == "chk0_p(X) :- p(X)." &&
                    subCheckRules[2].ToString() == "chk1_p :- q." &&
                    subCheckRules[3].ToString() == "chk1_p :- p." &&
                    subCheckRules[4].ToString() == "chk0_empty_head :- not q.");
    }
    
}
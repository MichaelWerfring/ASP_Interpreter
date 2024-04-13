using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Solving.NMRCheck;

public class NmrChecker
{
    public static List<Statement> GetSubCheckRules(List<Statement> olonRules)
    {
        List<Statement> subCheckRules = [];
        List<string> ruleNames = [];
        
        //foreach (var olonRule in olonRules)
        //{
        //    string ruleName = ASPExtensions.GenerateUniqeName(
        //        olonRule.Head?.Literal?.Identifier ?? "empty_head",
        //        ruleNames, "chk");
        //    
        //    
        //    foreach (var literal in olonRule.Body.Literals)
        //    {
        //        Statement subcheck = new();
        //        
        //        subcheck.AddHead(new Head(new ClassicalLiteral(
        //                ruleName,
        //                literal.ClassicalLiteral.Negated,
        //                literal.ClassicalLiteral.Terms
        //                )));
        //        
        //        subcheck.AddBody(new Body([
        //            new NafLiteral(new ClassicalLiteral(
        //                literal.ClassicalLiteral.Identifier,
        //                false,
        //                literal.ClassicalLiteral.Terms), !literal.IsNafNegated)
        //        ]));
        //        
        //        subCheckRules.Add(subcheck);
        //    }
        //}
        
        return subCheckRules;
    }
}
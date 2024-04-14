using asp_interpreter_lib.FileIO;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Solving.NMRCheck;

public class NmrChecker
{
    private static GoalToLiteralConverter _goalToLiteralConverter = new();

    public static List<Statement> GetSubCheckRules(List<Statement> olonRules)
    {
        DualRuleConverter converter = new DualRuleConverter(new AspProgram(olonRules,
                new Query(new Literal("dummy",
                    false,
                    false,
                    []))),
            new DualConverterOptions("rwh",
                "fa"));
        
        //Handle empty heads
        List<string>headNames = [];
        foreach (var olonRule in olonRules)
        {
            string headName;
            if (!olonRule.HasHead)
            {
                headName = ASPExtensions.GenerateUniqeName(string.Empty, headNames, "chk");
                olonRule.AddHead(new Literal(headName, false, false, []));
                continue;
            }
            
            headName = ASPExtensions.GenerateUniqeName(olonRule.Head.GetValueOrThrow().Identifier, headNames, "chk");
            olonRule.Head.GetValueOrThrow().Identifier = headName;
        }
        
        var duals = converter.GetDualRules(olonRules);
        
        List<Statement> subCheckRules = [];
        List<Literal> nmrCheckBody = [];
        foreach (var dual in duals)
        {
            var head = dual.Head.GetValueOrThrow("Head is missing in dual rule");

            //If it is not in the body of the nmr_check rule already add it
            if (nmrCheckBody.Find(l => l.Terms.Count == head.Terms.Count
                && l.Identifier == head.Identifier) == null)
            {
                nmrCheckBody.Add(head);
            }
            
            subCheckRules.Add(dual);
        }
        
        Statement nmrCheck = new();
        nmrCheck.AddHead(new Literal("nmr_check", false, false, []));
        nmrCheck.AddBody([]);
        
        nmrCheck.Body.AddRange(nmrCheckBody);
        subCheckRules.Insert(0, nmrCheck);
        return subCheckRules;
    }
    
    public static List<Statement> GetSubCheckRules1(List<Statement> olonRules)
    {
        List<Statement> subCheckRules = [];
        List<string> ruleNames = [];
        List<Literal> nmrCheckBody = [];
        
        foreach (var olonRule in olonRules)
        {
            string ruleName;
            if (olonRule.HasHead)
            {
                ruleName = ASPExtensions.GenerateUniqeName(
                    olonRule.Head.GetValueOrThrow().Identifier,
                    ruleNames, "chk");
            }
            else
            {
                ruleName = ASPExtensions.GenerateUniqeName(
                    string.Empty,
                    ruleNames, "chk");
            }
            
            foreach (var goal in olonRule.Body)
            {
                Statement subcheck = new();
                var literal = goal.Accept(_goalToLiteralConverter);

                if (!literal.HasValue)
                {
                    //Skip for now then it is
                    //something else than literal
                    continue;
                }

                var literalValue = literal.GetValueOrThrow();
                var newSubcheckHead = new Literal(
                    ruleName,
                    false,
                    literalValue.HasStrongNegation,
                    literalValue.Terms);
                subcheck.AddHead(newSubcheckHead);

                //If it is not in the body of the nmr_check rule already add it
                if (nmrCheckBody.Find(l => l.Terms.Count == newSubcheckHead.Terms.Count
                    && l.Identifier == newSubcheckHead.Identifier) == null)
                {
                    nmrCheckBody.Add(newSubcheckHead);
                }
                
                subcheck.AddBody([new Literal(
                    literalValue.Identifier,
                    !literalValue.HasNafNegation,
                    literalValue.HasStrongNegation,
                    literalValue.Terms)]);
                
                subCheckRules.Add(subcheck);
                
            }
        }
        Statement nmrCheck = new();
        nmrCheck.AddHead(new Literal("nmr_check", false, false, []));
        nmrCheck.AddBody([]);
        nmrCheck.Body.AddRange(nmrCheckBody);
        subCheckRules.Insert(0,nmrCheck);
        return subCheckRules;
    }
}
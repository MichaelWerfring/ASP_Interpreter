using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Solving.NMRCheck;

public class NmrChecker
{
    private static GoalToLiteralConverter _goalToLiteralConverter = new();

    public static List<Statement> GetSubCheckRules(List<Statement> olonRules)
    {
        List<Statement> subCheckRules = [];
        List<string> ruleNames = [];
        
        foreach (var olonRule in olonRules)
        {
            if (!olonRule.HasHead)
            {
                //Skip for now add forall here later
                continue;
            }
            
            
            string ruleName = ASPExtensions.GenerateUniqeName(
                olonRule.Head.GetValueOrThrow().Identifier ?? "empty_head",
                ruleNames, "chk");
            
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
                
                subcheck.AddHead(new Literal(
                    ruleName,
                    false, 
                    literal.GetValueOrThrow().HasStrongNegation,
                    literal.GetValueOrThrow().Terms));
                
                subcheck.AddBody([new Literal(
                    literal.GetValueOrThrow().Identifier,
                    !literal.GetValueOrThrow().HasNafNegation,
                    literal.GetValueOrThrow().HasStrongNegation,
                    literal.GetValueOrThrow().Terms)]);
                
                subCheckRules.Add(subcheck);
            }
        }
        
        return subCheckRules;
    }
}
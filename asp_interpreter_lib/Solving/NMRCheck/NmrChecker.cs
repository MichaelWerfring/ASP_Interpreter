using System.Reflection.Metadata.Ecma335;
using asp_interpreter_lib.FileIO;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Types.TypeVisitors.Copy;

namespace asp_interpreter_lib.Solving.NMRCheck;

public class NmrChecker
{
    private static GoalToLiteralConverter _goalToLiteralConverter = new();

    public static List<Statement> GetSubCheckRules(List<Statement> olonRules)
    {
        // 1) append negation of OLON Rule to its body (If not already present)
        // 2) generate dual for modified rule
        // 3) assign unique head (e.g. chk0)
        // 4) add modified duals to the NMR check goal and
        //    use forall to handle variables (e.g. nmr_check :- forall(X, chk0(X)).
        // ? headless rules have the forall applied when sub check is created (dual converter)
        
        DualRuleConverter converter = new DualRuleConverter(new AspProgram(olonRules,
                new Query(new Literal("dummy",
                    false,
                    false,
                    []))),
            new DualConverterOptions("rwh",
                "fa"), false);
        
        // 1) append negation of OLON Rule to its body (If not already present)
        foreach (var rule in olonRules)
        {
            if (!rule.HasHead)
            {
                continue;
            }
            
            var head = rule.Head.GetValueOrThrow("Could not parse head!");
            var negatedHead = head.Accept(new LiteralCopyVisitor(
                new TermCopyVisitor())).GetValueOrThrow("Could not parse negated head!");
            negatedHead.HasNafNegation = !negatedHead.HasNafNegation;
            
            bool containsHead = rule.Body.Find(b => b.ToString() == negatedHead.ToString()) != null;

            if (!containsHead)
            {
                rule.Body.Add(negatedHead);
            }
        }
        
        // 2) generate dual for modified rule
        var duals = converter.GetDualRules(olonRules);
        
        List<Literal> nmrCheckBody = [];
        // 3) assign unique head (e.g. chk0)
        for (var i = 0; i < duals.Count; i++)
        {
            var rule = duals[i];
            var head = rule.Head.GetValueOrThrow("Could not parse head!");
            head = new Literal(
                ASPExtensions.GenerateUniqeName(head.Identifier, new List<string>(), "chk"),
                false,
                head.HasStrongNegation,
                head.Terms
            );
            Statement newStatement = new();
            newStatement.AddHead(head);
            newStatement.AddBody(rule.Body);
            duals[i] = newStatement;

            // 4) add modified duals to the NMR check goal and
            if (nmrCheckBody.Find(b => b.ToString() == head.ToString()) == null)
            {
                nmrCheckBody.Add(head);
            }
        }

        Statement nmrCheck = new();
        nmrCheck.AddHead(new Literal("nmr_check", false, false, []));
        nmrCheck.AddBody([]);
        nmrCheck.Body.AddRange(nmrCheckBody);
        
        //just for readability add nmr_check to the beginning
        duals.Insert(0, nmrCheck);
        //Quantify variable in nmr_check
        
        
        return duals;
    }

    private static void AddForallToCheck(Statement statement)
    {
        //Variable are body variables implicitly
        List<string> variables = [];
        VariableFinder variableFinder = new();
        foreach (var goal in statement.Body)
        {
            variables.AddRange(goal.Accept(variableFinder).
                GetValueOrThrow("Cannot retrieve variables from body!").
                Select(v => v.Identifier));
        }

        for (var i = 0; i < statement.Body.Count; i++)
        {
            var innerGoal = statement.Body[i];
            var literal = innerGoal.Accept(_goalToLiteralConverter);

            if (!literal.HasValue)
            {
                continue;
            }

            var forall = DualRuleConverter.NestForall(variables.ToList(), literal.GetValueOrThrow());
            statement.Body[i] = forall;
        }
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
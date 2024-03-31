using System.Xml.XPath;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Solving;

public class DualRuleConverter
{
    public static List<Statement> GetDualRules(List<Statement> rules)
    {
        var duals = new List<Statement>();

        TransformHead(rules[0]);
        
        return duals;
    }

    private static List<Statement> TransformHeads(List<Statement> rules)
    {
        return rules;
    }
    
    public static Statement TransformHead(Statement rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        
        var terms = rule.Head.Literal?.Terms;
        if (terms == null) return rule;
        var visitor = new VariableTermVisitor();
        
        HashSet<string> variables = [];
        int count = terms.Count;
        
        for (var i = 0; i < count; i++)
        {
            var term = terms[i];
            var variableTerm = term.Accept(visitor);

            //If the term is not a variable it can be skipped
            if (!variableTerm.HasValue)
            {
                continue;
            }

            //If it occurs for the first time it can be skipped
            string current = variableTerm.GetValueOrThrow().Identifier;
            if (variables.Add(current))
            {
                continue;
            }

            var newVariable = new VariableTerm(
                ASPExtensions.GenerateVariableName(current, variables, "rwh"));

            //Rewrite the head
            terms[terms.IndexOf(term)] = newVariable;
            
            //Rewrite the body
            rule.Body.Literals.Add(new NafLiteral(new BinaryOperation(
                variableTerm.GetValueOrThrow(), new Equality(), newVariable)));
        }

        return rule;
    }
    
    public static AspProgram GetDualProgram(AspProgram initial)
    {
        var rules = initial.Statements;
        var duals = new List<Statement>();

        
        
        return new AspProgram(duals, initial.Query);
    }
}
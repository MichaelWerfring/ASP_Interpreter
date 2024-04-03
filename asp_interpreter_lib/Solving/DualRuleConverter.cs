using System.Reflection.Metadata;
using System.Xml.XPath;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using BasicTerm = asp_interpreter_lib.SimplifiedTerm.BasicTerm;
using VariableTerm = asp_interpreter_lib.Types.Terms.VariableTerm;

namespace asp_interpreter_lib.Solving;

public class DualRuleConverter
{
    public static Statement ReplaceDuplicateVariables(Statement rule)
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

            //If the term is not a variable it is replaced by a variable and unified with it
            if (!variableTerm.HasValue)
            {
                
                //Accept ToString/Null values for now
                var newHeadVariable = new VariableTerm(
                    ASPExtensions.GenerateVariableName(term.ToString() ?? "", variables, "rwh"));
                
                //replace head
                terms[terms.IndexOf(term)] = newHeadVariable;
                
                //replace body
                rule.Body.Literals.Insert(0,new NafLiteral(new BinaryOperation(
                    newHeadVariable, new Equality(), term)));
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
            rule.Body.Literals.Insert(0,new NafLiteral(new BinaryOperation(
                variableTerm.GetValueOrThrow(), new Equality(), newVariable)));
        }

        return rule;
    }

    //Just for easily handling entire programs
    public static List<Statement> GetDualRules(List<Statement> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);
        
        List<Statement> duals = [];

        foreach (var rule in rules)
        {
            duals.AddRange(GetDualRules(rule));
        }

        return duals;
    }
    
    public static List<Statement> GetDualRules(Statement rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        if (!rule.HasBody)
        {
            //Treat 
        }
        if (!rule.HasHead)
        {
            //Treat 
        }
        
        List<Statement> duals = [];

        var negator = new TypeNegator(new BinaryOperatorNegator(), new TermCopyVisitor());

        for (var i = 0; i < rule.Body.Literals.Count; i++)
        {
            var dualStatement = new Statement();
            
            //Maybe set negated on the classical literal in the head 
            //to indicate a dual rule
            dualStatement.AddHead(rule.Head);

            List<NafLiteral> dualBody = [];
            //add preceding
            dualBody.AddRange(rule.Body.Literals[0..i]);
            
            //add negated
            var goal = rule.Body.Literals[i];
            var negated = negator.NegateNaf(goal);
            dualBody.Add(negated);
            
            dualStatement.AddBody(new Body(dualBody));
            duals.Add(dualStatement);
        }

        return duals;
    }
    
    
    //Get all variables
    //VariableTermConverter variableConverter = new();
    //BasicTermConverter basicConverter = new();
    //List<VariableTerm> variables = [];
    //foreach (var c in term.Children)
    //{
    //    var variable = c.Accept(variableConverter);
    //
    //    if (variable.HasValue)
    //    {
    //        variables.Add(variable.GetValueOrThrow());
    //        continue;
    //    }
    //    
    //    //Should never throw due to the fact that there are only those two simplified terms
    //    ReplaceDuplicateVariables(c.Accept(basicConverter).GetValueOrThrow(
    //        "The given Terms children cannot be parsed to variable or basic terms!"));
    //}
    
    public static AspProgram GetDualProgram(AspProgram initial)
    {
        var rules = initial.Statements;
        var duals = new List<Statement>();

        return new AspProgram(duals, initial.Query);
    }
}
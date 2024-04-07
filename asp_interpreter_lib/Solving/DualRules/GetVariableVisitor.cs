using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Solving;

public class GetVariableVisitor : TypeBaseVisitor<HashSet<string>>
{
    private VariableTermConverter _converter = new VariableTermConverter();
    
    public override IOption<HashSet<string>> Visit(Head head)
    {
        ArgumentNullException.ThrowIfNull(head);

        if (head.Literal == null)
        {
            return new Some<HashSet<string>>([]);
        }
        
        HashSet<string> variables = [];

        foreach (var term in head.Literal.Terms)
        {
            term.Accept(_converter).
                IfHasValue(v =>
                    variables.Add(v.Identifier));
        }
        
        return new Some<HashSet<string>>(variables);
    }

    public override IOption<HashSet<string>> Visit(Body body)
    {
        ArgumentNullException.ThrowIfNull(body);
        HashSet<string> variables = [];

        foreach (var literal in body.Literals)
        {
            if (literal.IsBinaryOperation) continue;

            foreach (var term in literal.ClassicalLiteral.Terms)
            {
                term.Accept(_converter).
                    IfHasValue(v =>
                        variables.Add(v.Identifier));
            }
        }
        
        return new Some<HashSet<string>>(variables);
    }
}
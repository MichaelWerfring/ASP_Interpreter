using System.Text;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.Terms;

public class Forall : NafLiteral
{
    public VariableTerm VariableTerm { get; }
    public NafLiteral Goal { get; }

    public Forall(VariableTerm variableTerm, NafLiteral goal)
    {
        VariableTerm = variableTerm;
        Goal = goal;
    }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        string s = string.Empty;
        s += "forall(" + VariableTerm.ToString() + ", ";
        return s + Goal.ToString() + ")"; 
    }
}
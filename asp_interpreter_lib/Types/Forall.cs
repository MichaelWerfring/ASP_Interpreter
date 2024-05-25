using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types;

public class Forall : Goal
{
    public VariableTerm VariableTerm { get; }
    public Goal Goal { get; }

    public Forall(VariableTerm variableTerm, Goal goal)
    {
        VariableTerm = variableTerm;
        Goal = goal;
    }
    
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
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
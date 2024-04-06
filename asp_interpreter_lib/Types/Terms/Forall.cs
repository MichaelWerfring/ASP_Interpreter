using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.Terms;

public class Forall : ITerm
{
    public VariableTerm VariableTerm { get; }
    public ITerm Goal { get; }

    public Forall(VariableTerm variableTerm, ITerm goal)
    {
        VariableTerm = variableTerm;
        Goal = goal;
    }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;

public class VariableIntegerCase : IBinaryTermCase
{
    public VariableIntegerCase(Variable left, Integer right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        Left = left;
        Right = right;
    }

    public Variable Left { get; }

    public Integer Right { get; }

    public void Accept(IBinaryTermCaseVisitor visitor)
    {
        visitor.Visit(this);
    }

    public T Accept<T>(IBinaryTermCaseVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

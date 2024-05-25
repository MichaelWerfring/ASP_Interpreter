using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;

public class IntegerIntegerCase : IBinaryTermCase
{
    public IntegerIntegerCase(Integer left, Integer right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        Left = left;
        Right = right;
    }

    public Integer Left { get; }

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

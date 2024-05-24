using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;

public class VariableStructureCase : IBinaryTermCase
{
    public VariableStructureCase(Variable left, Structure right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        Left = left;
        Right = right;
    }
    public Variable Left { get; }

    public Structure Right { get; }

    public void Accept(IBinaryTermCaseVisitor visitor)
    {
        visitor.Visit(this);
    }

    public T Accept<T>(IBinaryTermCaseVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
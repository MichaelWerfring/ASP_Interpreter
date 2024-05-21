using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.Unification.Constructive.CaseDetermination.Cases;

public class VariableStructureCase : IBinaryTermCase
{
    public VariableStructureCase(Variable left, IStructure right)
    {
        Left = left;
        Right = right;
    }
    public Variable Left { get; }

    public IStructure Right { get; }

    public void Accept(IBinaryTermCaseVisitor visitor)
    {
        visitor.Visit(this);
    }
}
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.Unification.Constructive.CaseDetermination.Cases;

public class StructureVariableCase : IBinaryTermCase
{
    public StructureVariableCase(IStructure left, Variable right)
    {
        Left = left;
        Right = right;
    }

    public IStructure Left { get; }

    public Variable Right { get; }

    public void Accept(IBinaryTermCaseVisitor visitor)
    {
        visitor.Visit(this);
    }
}
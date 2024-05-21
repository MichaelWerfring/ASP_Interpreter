using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

namespace asp_interpreter_lib.Unification.Constructive.CaseDetermination.Cases;

public class StructureStructure : IBinaryTermCase
{
    public StructureStructure(IStructure left, IStructure right)
    {
        Left = left;
        Right = right;
    }

    public IStructure Left { get; }

    public IStructure Right { get; }

    public void Accept(IBinaryTermCaseVisitor visitor)
    {
        visitor.Visit(this);
    }
}
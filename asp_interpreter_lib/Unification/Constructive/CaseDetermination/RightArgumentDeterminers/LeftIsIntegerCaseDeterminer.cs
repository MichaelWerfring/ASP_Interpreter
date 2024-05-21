using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Unification.Constructive.CaseDetermination.Cases;

namespace asp_interpreter_lib.Unification.Constructive.CaseDetermination.RightDeterminer;

internal class LeftIsIntegerCaseDeterminer : ISimpleTermArgsVisitor<IBinaryTermCase, Integer>
{
    public IBinaryTermCase Visit(Variable right, Integer left)
    {
        return new StructureVariableCase(left, right);
    }

    public IBinaryTermCase Visit(Structure right, Integer left)
    {
        return new StructureStructure(left, right);
    }

    public IBinaryTermCase Visit(Integer right, Integer left)
    {
        return new StructureStructure(left, right);
    }
}
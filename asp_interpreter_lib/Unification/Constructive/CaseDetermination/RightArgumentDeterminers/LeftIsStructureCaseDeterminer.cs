using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Unification.Constructive.CaseDetermination.Cases;

namespace asp_interpreter_lib.Unification.Constructive.CaseDetermination.RightArgumentDeterminers;

internal class LeftIsStructureCaseDeterminer : ISimpleTermArgsVisitor<IBinaryTermCase, Structure>
{
    public IBinaryTermCase Visit(Variable right, Structure left)
    {
        return new StructureVariableCase(left, right);
    }

    public IBinaryTermCase Visit(Structure right, Structure left)
    {
        return new StructureStructure(left, right);
    }

    public IBinaryTermCase Visit(Integer right, Structure left)
    {
        return new StructureStructure(left, right);
    }
}
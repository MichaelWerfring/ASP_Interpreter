using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.RightArgumentDeterminers;

internal class LeftIsStructureCaseDeterminer : ISimpleTermArgsVisitor<IBinaryTermCase, Structure>
{
    public IBinaryTermCase Visit(Variable right, Structure left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new StructureVariableCase(left, right);
    }

    public IBinaryTermCase Visit(Structure right, Structure left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new StructureStructureCase(left, right);
    }

    public IBinaryTermCase Visit(Integer right, Structure left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new StructureIntegerCase(left, right);
    }
}
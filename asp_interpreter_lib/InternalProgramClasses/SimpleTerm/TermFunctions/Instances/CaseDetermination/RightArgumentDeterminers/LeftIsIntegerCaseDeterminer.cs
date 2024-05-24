using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.RightArgumentDeterminers;

internal class LeftIsIntegerCaseDeterminer : ISimpleTermArgsVisitor<IBinaryTermCase, Integer>
{
    public IBinaryTermCase Visit(Variable right, Integer left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new IntegerVariableCase(left, right);
    }

    public IBinaryTermCase Visit(Structure right, Integer left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new IntegerStructureCase(left, right);
    }

    public IBinaryTermCase Visit(Integer right, Integer left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new IntegerIntegerCase(left, right);
    }
}
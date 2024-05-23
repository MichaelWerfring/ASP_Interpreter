using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.RightArgumentDeterminers;

internal class LeftIsVariableCaseDeterminer : ISimpleTermArgsVisitor<IBinaryTermCase, Variable>
{
    public IBinaryTermCase Visit(Variable right, Variable left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new VariableVariableCase(left, right);
    }

    public IBinaryTermCase Visit(Structure right, Variable left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new VariableStructureCase(left, right);
    }

    public IBinaryTermCase Visit(Integer right, Variable left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new VariableIntegerCase(left, right);
    }
}
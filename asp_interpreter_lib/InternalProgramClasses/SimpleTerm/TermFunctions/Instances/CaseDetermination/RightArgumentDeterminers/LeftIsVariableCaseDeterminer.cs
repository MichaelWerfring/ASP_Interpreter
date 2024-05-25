using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.RightArgumentDeterminers;

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
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Unification.Constructive.CaseDetermination.Cases;

namespace asp_interpreter_lib.Unification.Constructive.CaseDetermination.RightDeterminer;

internal class LeftIsVariableCaseDeterminer : ISimpleTermArgsVisitor<IBinaryTermCase, Variable>
{
    public IBinaryTermCase Visit(Variable right, Variable left)
    {
        return new VariableVariableCase(left, right);
    }

    public IBinaryTermCase Visit(Structure right, Variable left)
    {
        return new VariableStructureCase(left, right);
    }

    public IBinaryTermCase Visit(Integer right, Variable left)
    {
        return new VariableStructureCase(left, right);
    }
}
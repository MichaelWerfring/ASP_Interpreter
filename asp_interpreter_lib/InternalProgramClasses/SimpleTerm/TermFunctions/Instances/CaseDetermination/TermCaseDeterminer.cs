using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.RightArgumentDeterminers;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination;

public class TermCaseDeterminer : ISimpleTermArgsVisitor<IBinaryTermCase, ISimpleTerm>
{
    private readonly LeftIsIntegerCaseDeterminer _rightCaseDeterminerForIntCase = new();
    private readonly LeftIsStructureCaseDeterminer _rightCaseDeterminerForStructureCase = new();
    private readonly LeftIsVariableCaseDeterminer _rightCaseDeterminerForVariableCase = new();

    public IBinaryTermCase DetermineCase(ISimpleTerm left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left.Accept(this, right);
    }

    public IBinaryTermCase Visit(Variable left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(_rightCaseDeterminerForVariableCase, left);
    }

    public IBinaryTermCase Visit(Structure left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(_rightCaseDeterminerForStructureCase, left);
    }

    public IBinaryTermCase Visit(Integer left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(_rightCaseDeterminerForIntCase, left);
    }
}
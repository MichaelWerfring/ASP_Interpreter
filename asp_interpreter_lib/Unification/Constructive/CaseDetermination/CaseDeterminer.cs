using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Unification.Constructive.CaseDetermination.Cases;
using asp_interpreter_lib.Unification.Constructive.CaseDetermination.RightArgumentDeterminers;
using asp_interpreter_lib.Unification.Constructive.CaseDetermination.RightDeterminer;

namespace asp_interpreter_lib.Unification.Constructive.CaseDetermination;

public class CaseDeterminer : ISimpleTermArgsVisitor<IBinaryTermCase, ISimpleTerm>
{
    private readonly LeftIsIntegerCaseDeterminer _leftIsIntCase = new();
    private readonly LeftIsStructureCaseDeterminer _leftIsStructCase = new();
    private readonly LeftIsVariableCaseDeterminer _leftIsVarCase = new();

    public IBinaryTermCase DetermineCase(ISimpleTerm left, ISimpleTerm right)
    {
        return left.Accept(this, right);
    }

    public IBinaryTermCase Visit(Variable left, ISimpleTerm right)
    {
        return right.Accept(_leftIsVarCase, left);
    }

    public IBinaryTermCase Visit(Structure left, ISimpleTerm right)
    {
        return right.Accept(_leftIsStructCase, left);
    }

    public IBinaryTermCase Visit(Integer left, ISimpleTerm right)
    {
        return right.Accept(_leftIsIntCase, left);
    }
}
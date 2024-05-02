using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Unification.Basic.Interfaces;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Unification.Basic.Robinson;

public class RobinsonUnificationAlgorithm : IUnificationAlgorithm
{
    private bool _doOccursCheck;

    public RobinsonUnificationAlgorithm(bool doOccursCheck)
    {
        _doOccursCheck = doOccursCheck;
    }

    public IOption<Dictionary<Variable, ISimpleTerm>> Unify(ISimpleTerm left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        var unifier = new RobinsonUnifier(_doOccursCheck);

        return unifier.Unify(left, right);
    }
}


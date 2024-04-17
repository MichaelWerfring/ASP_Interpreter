using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.Unification.Interfaces;

public interface IUnificationAlgorithm
{
    public IOption<Dictionary<Variable, ISimpleTerm>> Unify(ISimpleTerm left, ISimpleTerm right);
}

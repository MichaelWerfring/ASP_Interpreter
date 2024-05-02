using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Unification.Basic.Interfaces;

public interface IUnificationAlgorithm
{
    public IOption<Dictionary<Variable, ISimpleTerm>> Unify(ISimpleTerm left, ISimpleTerm right);
}

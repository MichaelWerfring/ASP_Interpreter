using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Unification.Interfaces
{
    public interface IUnificationAlgorithm
    {
        public IOption<Dictionary<VariableTerm, ITerm>> Unify(ITerm left, ITerm right);
    }
}

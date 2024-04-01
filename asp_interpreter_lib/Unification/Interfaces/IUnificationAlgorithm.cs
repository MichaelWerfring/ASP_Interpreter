using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.SimplifiedTerm;

namespace asp_interpreter_lib.Unification.Interfaces
{
    public interface IUnificationAlgorithm
    {
        public IOption<Dictionary<VariableTerm, ISimplifiedTerm>> Unify(ISimplifiedTerm left, ISimplifiedTerm right);
    }
}

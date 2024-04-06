using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;

namespace asp_interpreter_lib.Unification.Interfaces
{
    public interface IUnificationAlgorithm
    {
        public IOption<Dictionary<Variable, IInternalTerm>> Unify(IEnumerable<(IInternalTerm, IInternalTerm)> equations);
    }
}

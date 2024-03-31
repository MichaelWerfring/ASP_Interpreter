using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection.Rules
{
    public interface IMMRule
    {
        public IOption<IEnumerable<(ITerm, ITerm)>> ApplyRule((ITerm, ITerm) equation, IEnumerable<(ITerm, ITerm)> equations);
    }
}
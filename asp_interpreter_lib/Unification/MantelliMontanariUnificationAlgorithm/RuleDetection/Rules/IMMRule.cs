using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.SimplifiedTerm;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection.Rules
{
    public interface IMMRule
    {
        public IOption<IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)>> ApplyRule
        (
            (ISimplifiedTerm, ISimplifiedTerm) equation, 
            IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)> equations
        );
    }
}
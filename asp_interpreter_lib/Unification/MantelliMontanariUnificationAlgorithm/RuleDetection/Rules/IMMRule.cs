using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.RuleDetection.Rules
{
    public interface IMMRule
    {
        public IOption<IEnumerable<(IInternalTerm, IInternalTerm)>> ApplyRule
        (
            (IInternalTerm, IInternalTerm) equation,
            IEnumerable<(IInternalTerm, IInternalTerm)> equations
        );
    }
}
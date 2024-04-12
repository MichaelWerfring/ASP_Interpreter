using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;

namespace asp_interpreter_lib.Unification.MartelliMontanariUnificationAlgorithm.RuleDetection.Rules;

public interface IMMRule
{
    public IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> ApplyRule
    (
        (ISimpleTerm, ISimpleTerm) equation,
        IEnumerable<(ISimpleTerm, ISimpleTerm)> equations
    );
}
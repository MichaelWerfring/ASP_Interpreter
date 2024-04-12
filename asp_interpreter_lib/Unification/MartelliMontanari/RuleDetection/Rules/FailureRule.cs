using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;


namespace asp_interpreter_lib.Unification.MartelliMontanariUnificationAlgorithm.RuleDetection.Rules;

public class FailureRule : IMMRule
{
    public IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> ApplyRule
    (
        (ISimpleTerm, ISimpleTerm) equation,
        IEnumerable<(ISimpleTerm, ISimpleTerm)> equations
    )
    {
        ArgumentNullException.ThrowIfNull(equation);
        ArgumentNullException.ThrowIfNull(equations);

        return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
    }
}

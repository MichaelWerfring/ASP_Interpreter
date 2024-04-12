using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;

namespace asp_interpreter_lib.Unification.MartelliMontanariUnificationAlgorithm.RuleDetection.Rules;

public class RewriteRule : IMMRule
{
    public IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> ApplyRule
    (
        (ISimpleTerm, ISimpleTerm) equation,
        IEnumerable<(ISimpleTerm, ISimpleTerm)> equations
    )
    {
        ArgumentNullException.ThrowIfNull(equation);
        ArgumentNullException.ThrowIfNull(equations);

        var newEquations = equations.ToList();
        newEquations.Remove(equation);
        newEquations.Add((equation.Item2, equation.Item1));

        return new Some<IEnumerable<(ISimpleTerm, ISimpleTerm)>>(newEquations);
    }
}

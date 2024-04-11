using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;

namespace asp_interpreter_lib.Unification.MartelliMontanariUnificationAlgorithm.RuleDetection.Rules;

public class EliminationRule : IMMRule
{
    private SimpleTermReplacer _replacer;

    public EliminationRule()
    {
        _replacer = new SimpleTermReplacer();
    }

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
        newEquations = newEquations.Select
        (
            (eq) =>
            {
                ISimpleTerm left = _replacer.Replace(eq.Item1, equation.Item1, equation.Item2);
                ISimpleTerm right = _replacer.Replace(eq.Item2, equation.Item1, equation.Item2);

                return (left, right);
            }
        )
        .ToList();

        newEquations.Add(equation);

        return new Some<IEnumerable<(ISimpleTerm, ISimpleTerm)>>(newEquations);
    }
}

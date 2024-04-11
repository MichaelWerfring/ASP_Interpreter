using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;

namespace asp_interpreter_lib.Unification.MartelliMontanariUnificationAlgorithm.RuleDetection.Rules;

public class ReductionRule : IMMRule
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

        IEnumerable<(ISimpleTerm, ISimpleTerm)> terms;
        if (equation.Item1 is Structure a && equation.Item2 is Structure b)
        {
            terms = newEquations.Concat(ReduceStructures(a, b));
        }
        else
        {
            throw new ArgumentException("Must both be reducible terms");
        }

        return new Some<IEnumerable<(ISimpleTerm, ISimpleTerm)>>(terms.ToList());
    }


    private IEnumerable<(ISimpleTerm, ISimpleTerm)> ReduceStructures(Structure a, Structure b)
    {
        return a.Children.Zip(b.Children);
    }
}
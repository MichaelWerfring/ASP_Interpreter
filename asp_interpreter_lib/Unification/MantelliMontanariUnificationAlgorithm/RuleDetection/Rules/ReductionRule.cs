using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure = asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms.Structure;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.RuleDetection.Rules;

public class ReductionRule : IMMRule
{
    public IOption<IEnumerable<(IInternalTerm, IInternalTerm)>> ApplyRule
    (
        (IInternalTerm, IInternalTerm) equation,
        IEnumerable<(IInternalTerm, IInternalTerm)> equations
    )
    {
        ArgumentNullException.ThrowIfNull(equation);
        ArgumentNullException.ThrowIfNull(equations);

        var newEquations = equations.ToList();
        newEquations.Remove(equation);

        IEnumerable<(IInternalTerm, IInternalTerm)> terms = newEquations;
        if (equation.Item1 is Structure a && equation.Item2 is Structure b)
        {
            terms = newEquations.Concat(ReduceStructures(a, b));
        }
        else if (equation.Item1 is Integer aInt && equation.Item2 is Integer bInt)
        {
            terms = newEquations.Concat(ReduceIntegers(aInt, bInt));
        }
        else
        {
            throw new ArgumentException("Must both be reducible terms");
        }

        return new Some<IEnumerable<(IInternalTerm, IInternalTerm)>>(terms.ToList());
    }


    private IEnumerable<(IInternalTerm, IInternalTerm)> ReduceStructures(Structure a, Structure b)
    {
        return a.Children.Zip(b.Children);
    }

    private IEnumerable<(IInternalTerm, IInternalTerm)> ReduceIntegers(Integer a, Integer b)
    {
        return Enumerable.Empty<(IInternalTerm, IInternalTerm)>();
    }
}
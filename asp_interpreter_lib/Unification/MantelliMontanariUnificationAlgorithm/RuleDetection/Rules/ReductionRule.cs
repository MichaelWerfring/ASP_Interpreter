using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.SimplifiedTerm;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicTerm = asp_interpreter_lib.SimplifiedTerm.BasicTerm;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection.Rules
{
    public class ReductionRule : IMMRule
    {
        public IOption<IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)>> ApplyRule
        (
            (ISimplifiedTerm, ISimplifiedTerm) equation,
            IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)> equations
        )
        {
            ArgumentNullException.ThrowIfNull(equation);
            ArgumentNullException.ThrowIfNull(equations);
            if (!equations.Contains(equation))
            {
                throw new ArgumentException(nameof(equations), $"Must contain {nameof(equation)}");
            }
            BasicTerm left;
            BasicTerm right;
            try
            {
                left = (BasicTerm)equation.Item1;
                right = (BasicTerm)equation.Item2;
            }
            catch
            {
                throw new ArgumentException($"{nameof(equation.Item1)}, {nameof(equation.Item2)}, must both be of type {typeof(BasicTerm)}");
            }

            var newEquations = equations.ToList();
            newEquations.Remove(equation);
            for(int i = 0; i < left.Children.Count(); i++)
            {
                newEquations.Add((left.Children.ElementAt(i), right.Children.ElementAt(i)));
            }

            return new Some<IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)>>(newEquations);
        }
    }
}

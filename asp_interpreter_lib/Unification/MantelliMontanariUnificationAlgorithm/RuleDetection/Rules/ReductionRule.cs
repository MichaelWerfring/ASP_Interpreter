using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection.Rules
{
    public class ReductionRule : IMMRule
    {
        public IOption<IEnumerable<(ITerm, ITerm)>> ApplyRule((ITerm, ITerm) equation, IEnumerable<(ITerm, ITerm)> equations)
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

            if(left.Identifier != right.Identifier)
            {
                return new None<IEnumerable<(ITerm, ITerm)>>();
            }

            if (left.Terms.Count() != right.Terms.Count())
            {
                return new None<IEnumerable<(ITerm, ITerm)>>();
            }

            var newEquations = equations.ToList();
            newEquations.Remove(equation);

            for(int i = 0; i < left.Terms.Count(); i++)
            {
                newEquations.Add((left.Terms[i], right.Terms[i]));
            }

            return new Some<IEnumerable<(ITerm, ITerm)>>(newEquations);
        }
    }
}

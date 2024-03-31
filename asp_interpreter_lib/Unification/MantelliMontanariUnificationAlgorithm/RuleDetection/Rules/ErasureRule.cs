using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection.Rules
{
    public class ErasureRule : IMMRule
    {
        public IOption<IEnumerable<(ITerm, ITerm)>> ApplyRule((ITerm, ITerm) equation, IEnumerable<(ITerm, ITerm)> equations)
        {
            ArgumentNullException.ThrowIfNull(equation);
            ArgumentNullException.ThrowIfNull(equations);
            if (!equations.Contains(equation))
            {
                throw new ArgumentException(nameof(equations), $"Must contain {nameof(equation)}");
            }

            var newEquations = equations.ToList();

            newEquations.Remove(equation);

            return new Some<IEnumerable<(ITerm, ITerm)>>(newEquations);
        }
    }
}

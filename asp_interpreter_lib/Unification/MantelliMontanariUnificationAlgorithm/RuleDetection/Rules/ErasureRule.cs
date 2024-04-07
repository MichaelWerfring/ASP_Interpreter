using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.RuleDetection.Rules
{
    public class ErasureRule : IMMRule
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

            return new Some<IEnumerable<(IInternalTerm, IInternalTerm)>>(newEquations);
        }
    }
}

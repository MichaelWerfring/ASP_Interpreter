using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TermFunctionality;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection.Rules
{
    public class EliminationRule : IMMRule
    {
        private TermContainsChecker _termContainsChecker;
        private TermReplacer _replacer;

        public EliminationRule()
        {
            _termContainsChecker = new TermContainsChecker();
            _replacer = new TermReplacer();
        }

        public IOption<IEnumerable<(ITerm, ITerm)>> ApplyRule((ITerm, ITerm) equation, IEnumerable<(ITerm, ITerm)> equations)
        {
            ArgumentNullException.ThrowIfNull(equation);
            ArgumentNullException.ThrowIfNull(equations);
            if (!equations.Contains(equation))
            {
                throw new ArgumentException(nameof(equations), $"Must contain {nameof(equation)}");
            }

            if (_termContainsChecker.LeftContainsRight(equation.Item2, equation.Item1))
            {
                return new None<IEnumerable<(ITerm, ITerm)>>();
            }

            var newEquations = equations.ToList();
            newEquations.Remove(equation);

            var nu = newEquations.Select((eq) =>
            {
                ITerm left = _replacer.Replace(eq.Item1, equation.Item1, equation.Item2);
                ITerm right = _replacer.Replace(eq.Item1, equation.Item1, equation.Item2);

                return (left, right);
            });

            newEquations.Add(equation);

            return new Some<IEnumerable<(ITerm,ITerm)>>(newEquations);
        }
    }
}

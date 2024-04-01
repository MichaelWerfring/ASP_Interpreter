using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.SimplifiedTerm;
using asp_interpreter_lib.SimplifiedTerm.TermFunctionality;
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

            var newEquations = equations.ToList();
            newEquations.Remove(equation);
            newEquations= newEquations.Select
            (
                (eq) =>
                {
                    ISimplifiedTerm left = _replacer.Replace(eq.Item1, equation.Item1, equation.Item2);
                    ISimplifiedTerm right = _replacer.Replace(eq.Item2, equation.Item1, equation.Item2);

                    return (left, right);
                }
            )
            .ToList();

            newEquations.Add(equation);

            return new Some<IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)>>(newEquations);
        }
    }
}

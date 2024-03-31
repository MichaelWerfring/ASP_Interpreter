using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Unification.Interfaces;
using asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection;
using asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm
{
    public class MMUnificationAlgorithm : IUnificationAlgorithm
    {
        private RuleMapper _rulesMapper;

        public MMUnificationAlgorithm(bool doOccursCheck)
        {
            _rulesMapper = new RuleMapper(doOccursCheck);
        }

        public IOption<Dictionary<VariableTerm, ITerm>> Unify(ITerm left, ITerm right)
        {
            ArgumentNullException.ThrowIfNull(left, nameof(left));
            ArgumentNullException.ThrowIfNull(right, nameof(right));

            List<(ITerm, ITerm)> initialEquations = new List<(ITerm, ITerm)>()
            {
                (left, right)
            };

            var resultsList = new List<IEnumerable<(ITerm, ITerm)>>();

            DoUnification(initialEquations, resultsList);

            if(resultsList.Count != 1)
            {
                return new None<Dictionary<VariableTerm, ITerm>>();
            }

            var equations = resultsList[0];

            var mapping = new Dictionary<VariableTerm, ITerm>();

            foreach( var equation in equations)
            {
                if(equation.Item1.GetType() != typeof(VariableTerm))
                {
                    Console.Write("Error: not a variable term");
                }

                VariableTerm variable = equation.Item1 as VariableTerm;

                mapping.Add(variable, equation.Item2);
            }

            return new Some<Dictionary<VariableTerm, ITerm>>(mapping);
        }

        private void DoUnification(IEnumerable<(ITerm, ITerm)> currentEquations, List<IEnumerable<(ITerm, ITerm)>> finalList)
        {
            foreach (var equation in currentEquations)
            {
                var ruleToApplyMaybe = _rulesMapper.GetAppropriateRule(equation, currentEquations);

                IMMRule rule;
                try
                {
                    rule = ruleToApplyMaybe.GetValueOrThrow();
                }
                catch
                {
                    continue;
                }

                var newEquationsMaybe = rule.ApplyRule(equation, currentEquations);
                IEnumerable<(ITerm, ITerm)> newEquations;
                try
                {
                    newEquations = newEquationsMaybe.GetValueOrThrow();
                }
                catch
                {
                    return;
                }

                DoUnification(newEquations, finalList);
                return;
            }

            finalList.Add(currentEquations);
        }
    }
}

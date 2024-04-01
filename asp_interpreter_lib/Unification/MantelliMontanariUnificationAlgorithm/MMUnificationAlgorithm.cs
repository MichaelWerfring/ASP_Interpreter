using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.SimplifiedTerm;
using asp_interpreter_lib.Unification.Interfaces;
using asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection;
using asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection.Rules;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm
{
    public class MMUnificationAlgorithm : IUnificationAlgorithm
    {
        private RuleMatcher _rulesMapper;

        public MMUnificationAlgorithm(bool doOccursCheck)
        {
            _rulesMapper = new RuleMatcher(doOccursCheck);
        }

        public IOption<Dictionary<VariableTerm, ISimplifiedTerm>> Unify(ISimplifiedTerm left, ISimplifiedTerm right)
        {
            ArgumentNullException.ThrowIfNull(left, nameof(left));
            ArgumentNullException.ThrowIfNull(right, nameof(right));

            List<(ISimplifiedTerm, ISimplifiedTerm)> initialEquations = new List<(ISimplifiedTerm, ISimplifiedTerm)>()
            {
                (left, right)
            };

            var resultsList = new List<IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)>>();

            DoUnification(initialEquations, resultsList);

            if(resultsList.Count != 1)
            {
                return new None<Dictionary<VariableTerm, ISimplifiedTerm>>();
            }

            var equations = resultsList[0];

            var mapping = new Dictionary<VariableTerm, ISimplifiedTerm>();
            foreach( var equation in equations)
            {
                if(equation.Item1.GetType() != typeof(VariableTerm))
                {
                    Console.Write("Error: not a variable term");
                }

                VariableTerm variable = (VariableTerm) equation.Item1;

                mapping.Add(variable, equation.Item2);
            }

            return new Some<Dictionary<VariableTerm, ISimplifiedTerm>>(mapping);
        }

        private void DoUnification(IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)> currentEquations, List<IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)>> finalList)
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
                IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)> newEquations;
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

using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.Unification.Interfaces;
using asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.RuleDetection;
using asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.RuleDetection.Rules;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm
{
    public class MMUnificationAlgorithm : IUnificationAlgorithm
    {
        private RuleMatcher _rulesMapper;

        public MMUnificationAlgorithm(bool doOccursCheck)
        {
            _rulesMapper = new RuleMatcher(doOccursCheck);
        }

        public IOption<Dictionary<Variable, IInternalTerm>> Unify(IEnumerable<(IInternalTerm, IInternalTerm)> initialEquations)
        {
            ArgumentNullException.ThrowIfNull(initialEquations, nameof(initialEquations));

            var resultsList = new List<IEnumerable<(IInternalTerm, IInternalTerm)>>();

            bool search = true;
            DoUnification(initialEquations, resultsList, ref search);

            if (resultsList.Count != 1)
            {
                return new None<Dictionary<Variable, IInternalTerm>>();
            }

            var equations = resultsList[0];

            var mapping = new Dictionary<Variable, IInternalTerm>();
            foreach (var equation in equations)
            {
                if (equation.Item1.GetType() != typeof(Variable))
                {
                    throw new Exception("Not a variable term!");
                }

                Variable variable = (Variable)equation.Item1;

                mapping.Add(variable, equation.Item2);
            }

            return new Some<Dictionary<Variable, IInternalTerm>>(mapping);
        }

        private void DoUnification
        (
            IEnumerable<(IInternalTerm, IInternalTerm)> currentEquations, 
            List<IEnumerable<(IInternalTerm, IInternalTerm)>> finalList,
            ref bool doSearch
            )
        {
            foreach (var equation in currentEquations)
            {
                if (!doSearch)
                {
                    return;
                }

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
                IEnumerable<(IInternalTerm, IInternalTerm)> newEquations;
                try
                {
                    newEquations = newEquationsMaybe.GetValueOrThrow();
                }
                catch
                {
                    return;
                }

                DoUnification(newEquations, finalList, ref doSearch);
                return;
            }
            doSearch = false;
            finalList.Add(currentEquations);
        }
    }
}

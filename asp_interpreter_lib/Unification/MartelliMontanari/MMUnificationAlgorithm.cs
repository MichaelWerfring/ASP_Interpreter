using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Unification.Interfaces;
using asp_interpreter_lib.Unification.MantelliMontanari.RuleDetection;
using asp_interpreter_lib.Unification.MartelliMontanariUnificationAlgorithm.RuleDetection.Rules;

namespace asp_interpreter_lib.Unification.MartelliMontanariUnificationAlgorithm;

public class MMUnificationAlgorithm : IUnificationAlgorithm
{
    private RuleMatcher _rulesMapper;

    public MMUnificationAlgorithm(bool doOccursCheck)
    {
        _rulesMapper = new RuleMatcher(doOccursCheck);
    }

    public IOption<Dictionary<Variable, ISimpleTerm>> Unify(ISimpleTerm left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        var initialList = new List<(ISimpleTerm, ISimpleTerm)>() { (left, right)};

        var resultsList = new List<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();

        bool search = true;
        DoUnification(initialList, resultsList, ref search);

        if (resultsList.Count != 1)
        {
            return new None<Dictionary<Variable, ISimpleTerm>>();
        }

        var equations = resultsList[0];

        var mapping = new Dictionary<Variable, ISimpleTerm>();
        foreach (var equation in equations)
        {
            if (equation.Item1 is Variable var)
            {
                mapping.Add(var, equation.Item2);
            }
            else
            {
                throw new Exception("Not a variable term!");
            }
        }

        return new Some<Dictionary<Variable, ISimpleTerm>>(mapping);
    }

    private void DoUnification
    (
        IEnumerable<(ISimpleTerm, ISimpleTerm)> currentEquations, 
        List<IEnumerable<(ISimpleTerm, ISimpleTerm)>> finalList,
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
            IEnumerable<(ISimpleTerm, ISimpleTerm)> newEquations;
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

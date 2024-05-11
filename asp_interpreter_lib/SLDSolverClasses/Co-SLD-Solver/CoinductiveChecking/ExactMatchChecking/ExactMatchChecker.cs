using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;

public class ExactMatchChecker
{
    private IConstructiveUnificationAlgorithm _algorithm;

    public ExactMatchChecker(IConstructiveUnificationAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(algorithm);

        _algorithm = algorithm;
    }

    public bool AreExactMatch(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        // if they dont unify at all, then they are not an exact match.
        var unificationMaybe = _algorithm.Unify(target);
        if (!unificationMaybe.HasValue)
        {
            return false;
        }

        var unification = unificationMaybe.GetValueOrThrow();

        // if the new mapping contains term bindings, then they are not an exact match.
        Dictionary<Variable, ProhibitedValuesBinding> newProhibitedValuesMapping;
        try
        {
            newProhibitedValuesMapping = unification.Mapping
                .Select(x => (x.Key, (ProhibitedValuesBinding)x.Value))
                .ToDictionary(new VariableComparer());
        }
        catch 
        {
            return false;
        }

        // if for any variable : their old and new prohibited values are different
        if 
        (
            target.Mapping.Keys.Any(x =>
            {
                var oldProhibitedValues = target.Mapping[x].ProhibitedValues;
                var newProhibitedValues = newProhibitedValuesMapping[x].ProhibitedValues;

                if (oldProhibitedValues.Count != newProhibitedValues.Count)
                {
                    return true;
                }

                var intersection = oldProhibitedValues
                .Intersect(newProhibitedValues, new SimpleTermEqualityComparer());

                if (intersection.Count() != oldProhibitedValues.Count)
                {
                    return true;
                }

                return false;
            })
        )
        {
            return false;
        }

        return true;
    }
}

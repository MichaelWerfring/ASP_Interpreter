using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;

public class ExactMatchChecker
{
    private TransitiveVariableMappingResolver _simplifier = new TransitiveVariableMappingResolver(true);

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
        VariableMapping unification;
        try
        {
            unification = _algorithm.Unify(target).GetValueOrThrow();
        }
        catch
        {
            return false;
        }

        // extract variables from both input terms.
        var variables = target.Left.ExtractVariables()
                        .Union(target.Right.ExtractVariables(), new VariableComparer());

        // transitively resolve variable mappings.
        var variablesToTransitiveMapping = variables
            .Select(var => (var, _simplifier.Resolve(var, unification)))
            .ToDictionary(new VariableComparer());

        // get the prohibitedValueBindings transitively: this is necessary because through constructive unification, there could be cases such as:
        // X => Y => \= {1, 2}.
        // if there are termbindings, then no exact match.
        Dictionary<Variable, ProhibitedValuesBinding> newProhibitedValuesMapping;
        try
        {
            newProhibitedValuesMapping = variablesToTransitiveMapping
            .Select(pair => (pair.Key, (ProhibitedValuesBinding)pair.Value))
            .ToDictionary(new VariableComparer());
        }
        catch
        {
            return false;
        }

        // if for any variable:
        // their old and new prohibited values are different, then no match.
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

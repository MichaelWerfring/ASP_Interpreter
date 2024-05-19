using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
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

        // Transitively resolve: this is necessary
        // because through constructive unification, there could be cases such as:
        // X => Y => \= {1, 2}.
        // if there are termbindings, then no exact match.
        var variablesToTransitiveMapping = variables
                                           .Select(var => (var, unification.Resolve(var, true)))
                                           .ToDictionary(new VariableComparer());

        // get only the prohibited values: if there are any termbinding, then return false.
        Dictionary<Variable, ProhibitedValuesBinding> newProhibitedValueBindings;
        try
        {
            newProhibitedValueBindings = variablesToTransitiveMapping
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
            variables.Any(x =>
            {
                var olds = target.Mapping[x].ProhibitedValues;
                var news = newProhibitedValueBindings[x].ProhibitedValues;

                if (olds.Count != news.Count)
                {
                    return true;
                }

                var intersection = olds.Intersect(news);

                if (intersection.Count() != olds.Count)
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

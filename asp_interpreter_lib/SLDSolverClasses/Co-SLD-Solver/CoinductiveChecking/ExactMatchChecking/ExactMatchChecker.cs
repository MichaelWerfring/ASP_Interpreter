// <copyright file="ExactMatchChecker.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Unification.Constructive.Unification;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

/// <summary>
/// A class for checking whether a constructive target unifies with an exact match.
/// </summary>
public class ExactMatchChecker
{
    private readonly IConstructiveUnificationAlgorithm algorithm;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExactMatchChecker"/> class.
    /// </summary>
    /// <param name="algorithm">The unification algorithm to use.</param>
    public ExactMatchChecker(IConstructiveUnificationAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(algorithm);

        this.algorithm = algorithm;
    }

    /// <summary>
    /// Checks if the target unifies with an exact match.
    /// </summary>
    /// <param name="target">The target to check.</param>
    /// <returns>A value indicating whether the target is an exact match.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="target"/> is null.</exception>
    public bool AreExactMatch(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        // if they dont unify at all, then they are not an exact match.
        IOption<VariableMapping> unificationMaybe = this.algorithm.Unify(target);

        if (!unificationMaybe.HasValue)
        {
            return false;
        }

        VariableMapping unification = unificationMaybe.GetValueOrThrow();

        // extract variables from both input terms.
        var variables = target.Left.ExtractVariables()
                        .Union(target.Right.ExtractVariables(), TermFuncs.GetSingletonVariableComparer());

        // Transitively resolve: this is necessary
        // because through constructive unification, there could be cases such as:
        // X => Y => \= {1, 2}.
        var varsToResolvedValues = variables.AsParallel().Select(
            var => new KeyValuePair<Variable, IVariableBinding>(var, unification.Resolve(var, true).GetValueOrThrow()));

        // wrap into variablemapping for convenience.
        var mapping = new VariableMapping(varsToResolvedValues.ToImmutableDictionary(TermFuncs.GetSingletonVariableComparer()));

        // old prohibited values are just the ones in the target.
        var oldProhibs = target.Mapping;

        // get only prohibitedValues.
        var newProhibs = mapping.GetProhibitedValueBindings();

        // if new mapping contains termbindings, then they cannot be an exact match.
        if (newProhibs.Count != mapping.Count)
        {
            return false;
        }

        // if for any variable:
        // their old and new prohibited values are different, then no match.
        if
        (
            variables.Any(variable =>
            {
                ImmutableSortedSet<ISimpleTerm> olds = oldProhibs[variable].ProhibitedValues;
                ImmutableSortedSet<ISimpleTerm> news = newProhibs[variable].ProhibitedValues;

                if (olds.Count != news.Count)
                {
                    return true;
                }

                var intersection = olds.Intersect(news);

                if (intersection.Count != olds.Count)
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
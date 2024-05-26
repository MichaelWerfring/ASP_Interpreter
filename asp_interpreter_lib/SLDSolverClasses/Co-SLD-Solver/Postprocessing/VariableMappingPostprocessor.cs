// <copyright file="VariableMappingPostprocessor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

/// <summary>
/// A class for postprocessing a variable mapping.
/// </summary>
public class VariableMappingPostprocessor
{
    /// <summary>
    /// Postprocess a variable mapping.
    /// </summary>
    /// <param name="mapping">The mapping to postprocess.</param>
    /// <returns>The postprocessed mapping.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mapping"/> is null.</exception>
    public VariableMapping Postprocess(VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        return this.Postprocess(mapping, mapping.Keys.Where(x => !x.Identifier.StartsWith('#')));
    }

    /// <summary>
    /// Postprocess a variable mapping, with an additional input signifying the variables to keep.
    /// </summary>
    /// <param name="map">The mapping to postprocess.</param>
    /// <param name="variablesToKeep">The variables to keep.</param>
    /// <returns>The postprocessed mapping.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="map"/> is null,
    /// ..<paramref name="variablesToKeep"/> is null.</exception>
    public VariableMapping Postprocess(VariableMapping map, IEnumerable<Variable> variablesToKeep)
    {
        ArgumentNullException.ThrowIfNull(map, nameof(map));
        ArgumentNullException.ThrowIfNull(variablesToKeep, nameof(variablesToKeep));

        // make a new binding, add variables and their simplified terms.
        var newBinding = new Dictionary<Variable, IVariableBinding>(TermFuncs.GetSingletonVariableComparer());
        foreach (var variable in variablesToKeep)
        {
            newBinding.Add(variable, map.Resolve(variable, true).GetValueOrThrow());
        }

        // get all variables from all the term bindings.
        var internalVariablesInTerms = newBinding.Values
            .OfType<TermBinding>()
            .SelectMany(termBinding => termBinding.Term.ExtractVariables())
            .Where(variable => variable.Identifier.StartsWith('#'))
            .ToImmutableHashSet(TermFuncs.GetSingletonVariableComparer());

        // for all the variables in the termbindings: add their values as well, if they have any.
        foreach (var variable in internalVariablesInTerms)
        {
            if (map.TryGetValue(variable, out IVariableBinding? bindings))
            {
                newBinding.TryAdd(variable, bindings);
            }
        }

        return new VariableMapping(newBinding.ToImmutableDictionary(TermFuncs.GetSingletonVariableComparer()));
    }
}
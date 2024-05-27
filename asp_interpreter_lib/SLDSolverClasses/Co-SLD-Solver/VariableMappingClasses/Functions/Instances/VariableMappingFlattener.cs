// <copyright file="VariableMappingFlattener.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

/// <summary>
/// A class for flattening a variable mapping by transitively resolving each value.
/// </summary>
internal class VariableMappingFlattener
{
    /// <summary>
    /// Flattens a mapping by transitively resolving each value.
    /// </summary>
    /// <param name="mapping">The input mapping.</param>
    /// <returns>The flattened mapping.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mapping"/> is null.</exception>
    public VariableMapping Flatten(VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        var newMapping = new KeyValuePair<Variable, IVariableBinding>[mapping.Count];

        Parallel.For(0, mapping.Count, index =>
        {
            var currentVariable = mapping.Keys.ElementAt(index);

            newMapping[index] = new KeyValuePair<Variable, IVariableBinding>(
                currentVariable, mapping.Resolve(currentVariable, false).GetValueOrThrow());
        });

        return new VariableMapping(newMapping.ToImmutableDictionary(TermFuncs.GetSingletonVariableComparer()));
    }
}
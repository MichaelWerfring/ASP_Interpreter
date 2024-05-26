// <copyright file="VariableMappingSplitter.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.Splitter;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

/// <summary>
/// A class for splitting a <see cref="VariableMapping"/> into a mapping of <see cref="Variable"/> to <see cref="TermBinding"/>
/// and a mapping of <see cref="Variable"/> to <see cref="ProhibitedValuesBinding"/>.
/// </summary>
public class VariableMappingSplitter
{
    private readonly ProhibitedValuesChecker prohibsFilterer = new();
    private readonly TermBindingChecker tbFilterer = new();

    /// <summary>
    /// Extracts the term bindings from a mapping.
    /// </summary>
    /// <param name="mapping">The mapping to split.</param>
    /// <returns>Only the term bindings.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mapping"/> is null.</exception>
    public IImmutableDictionary<Variable, TermBinding> GetTermBindings(VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        var filteredPairs = mapping
            .Select(pair => (pair.Key, this.tbFilterer.ReturnTermbindingOrNone(pair.Value)))
            .Where(pair => pair.Item2.HasValue)
            .Select(pair => new KeyValuePair<Variable, TermBinding>(pair.Key, pair.Item2.GetValueOrThrow()))
            .ToImmutableDictionary(TermFuncs.GetSingletonVariableComparer());

        return filteredPairs;
    }

    /// <summary>
    /// Extracts the prohibited value bindings from a mapping.
    /// </summary>
    /// <param name="mapping">The mapping to split.</param>
    /// <returns>Only the prohibited value bindings.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mapping"/> is null.</exception>
    public IImmutableDictionary<Variable, ProhibitedValuesBinding> GetProhibitedValueBindings(VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        var filteredPairs = mapping
            .Select(pair => (pair.Key, this.prohibsFilterer.ReturnProhibitedValueBindingOrNone(pair.Value)))
            .Where(pair => pair.Item2.HasValue)
            .Select(pair => new KeyValuePair<Variable, ProhibitedValuesBinding>(pair.Key, pair.Item2.GetValueOrThrow()))
            .ToImmutableDictionary(TermFuncs.GetSingletonVariableComparer());

        return filteredPairs;
    }
}
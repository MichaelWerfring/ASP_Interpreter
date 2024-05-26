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

public class VariableMappingSplitter
{
    private readonly ProhibitedValuesChecker _prohibsFilterer = new();
    private readonly TermBindingChecker _tbFilterer = new();

    public IImmutableDictionary<Variable, TermBinding> GetTermBindings(VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        var filteredPairs = mapping
            .Select(pair => (pair.Key, _tbFilterer.ReturnTermbindingOrNone(pair.Value)))
            .Where(pair => pair.Item2.HasValue)
            .Select(pair => new KeyValuePair<Variable, TermBinding>(pair.Key, pair.Item2.GetValueOrThrow()))
            .ToImmutableDictionary(TermFuncs.GetSingletonVariableComparer());

        return filteredPairs;
    }

    public IImmutableDictionary<Variable, ProhibitedValuesBinding> GetProhibitedValueBindings(VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        var filteredPairs = mapping
            .Select(pair => (pair.Key, _prohibsFilterer.ReturnProhibitedValueBindingOrNone(pair.Value)))
            .Where(pair => pair.Item2.HasValue)
            .Select(pair => new KeyValuePair<Variable, ProhibitedValuesBinding>(pair.Key, pair.Item2.GetValueOrThrow()))
            .ToImmutableDictionary(TermFuncs.GetSingletonVariableComparer());

        return filteredPairs;
    }
}

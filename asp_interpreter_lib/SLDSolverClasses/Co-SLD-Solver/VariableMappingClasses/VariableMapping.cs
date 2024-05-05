﻿using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using System.Collections.Immutable;

namespace asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

/// <summary>
/// Represents a mapping for the co-sld-resolution:
/// Variables are mapped to either a prohibited value set or a single term.
/// Wraps the dictionary to make sure it has the correct comparer.
/// </summary>
public class VariableMapping
{
    public VariableMapping(IImmutableDictionary<Variable, IVariableBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        Mapping = mapping;
    }

    public IImmutableDictionary<Variable, IVariableBinding> Mapping { get; }
}
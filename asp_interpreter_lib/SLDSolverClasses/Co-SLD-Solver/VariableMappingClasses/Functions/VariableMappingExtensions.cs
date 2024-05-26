// <copyright file="VariableMappingExtensions.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

internal static class VariableMappingExtensions
{
    private static readonly VariableMappingSplitter _splitter = new();
    private static readonly VariableMappingSubstituter _substituter = new();
    private static readonly VariableMappingFlattener _flattener = new();
    private static readonly VariableMappingUpdater _updater = new();
    private static readonly TransitiveVariableMappingResolver _toProhibResolver = new(true);
    private static readonly TransitiveVariableMappingResolver _toLastVariableResolver = new(false);

    public static IImmutableDictionary<Variable, TermBinding> GetTermBindings(this VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        return _splitter.GetTermBindings(mapping);
    }

    public static IImmutableDictionary<Variable, ProhibitedValuesBinding> GetProhibitedValueBindings(this VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        return _splitter.GetProhibitedValueBindings(mapping);
    }

    public static VariableMapping Flatten(this VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        return _flattener.Flatten(mapping);
    }

    public static IOption<VariableMapping> Update(this VariableMapping left, VariableMapping right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        return _updater.Update(left, right);
    }

    public static IOption<IVariableBinding> Resolve(this VariableMapping mapping, Variable var, bool doProhibitedValueResolution)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        ArgumentNullException.ThrowIfNull(var, nameof(var));

        if (doProhibitedValueResolution)
        {
            return _toProhibResolver.Resolve(var, mapping);
        }
        else
        {
            return _toLastVariableResolver.Resolve(var, mapping);
        }
    }

    public static ISimpleTerm ApplySubstitution(this VariableMapping mapping, ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        return _substituter.SubstituteTerm(term, mapping);
    }

    public static Structure ApplySubstitution(this VariableMapping mapping, Structure term)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        return _substituter.SubstituteStructure(term, mapping);
    }
}

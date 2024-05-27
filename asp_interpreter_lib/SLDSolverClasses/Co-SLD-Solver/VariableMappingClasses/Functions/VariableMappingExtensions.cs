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

/// <summary>
/// An extension class for <see cref="VariableMapping"/>.
/// </summary>
internal static class VariableMappingExtensions
{
    private static readonly VariableMappingSplitter Splitter = new();
    private static readonly VariableMappingSubstituter Substituter = new();
    private static readonly VariableMappingFlattener Flattener = new();
    private static readonly VariableMappingUpdater Updater = new();
    private static readonly TransitiveVariableMappingResolver ToProhibResolver = new(true);
    private static readonly TransitiveVariableMappingResolver ToLastVariableResolver = new(false);

    /// <summary>
    /// Extracts the term bindings from a mapping.
    /// </summary>
    /// <param name="mapping">The mapping to split.</param>
    /// <returns>Only the term bindings.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mapping"/> is null.</exception>
    public static IImmutableDictionary<Variable, TermBinding> GetTermBindings(this VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        return Splitter.GetTermBindings(mapping);
    }

    /// <summary>
    /// Extracts the prohibited value bindings from a mapping.
    /// </summary>
    /// <param name="mapping">The mapping to split.</param>
    /// <returns>Only the prohibited value bindings.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mapping"/> is null.</exception>
    public static IImmutableDictionary<Variable, ProhibitedValuesBinding> GetProhibitedValueBindings(this VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        return Splitter.GetProhibitedValueBindings(mapping);
    }

    /// <summary>
    /// Flattens a mapping by transitively resolving each value.
    /// </summary>
    /// <param name="mapping">The input mapping.</param>
    /// <returns>The flattened mapping.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mapping"/> is null.</exception>
    public static VariableMapping Flatten(this VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        return Flattener.Flatten(mapping);
    }

    /// <summary>
    /// Updates <see cref="VariableMapping"/> left by values in <see cref="VariableMapping"/> right like so:
    /// For every variable key in the union of left and right:
    /// if only left has value, take left.
    /// if only right has value, take right.
    /// if left and right has value:
    /// if left is prohib and right is prohib, take their union.
    /// if left is prohib and right is term, take right.
    /// if left is term and right is prohib, fail.
    /// if left is term and right is term, fail if they are different or just take right.
    /// </summary>
    /// <param name="left">The mapping to update.</param>
    /// <param name="right">The mapping to update with.</param>
    /// <returns>The updated mapping, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="left"/> is null,
    /// ..<paramref name="right"/> is null.</exception>
    public static IOption<VariableMapping> Update(this VariableMapping left, VariableMapping right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        return Updater.Update(left, right);
    }

    /// <summary>
    /// Transitively simplifies a variableBinding, ie. if X => Y => s(A, Z), and A -> a, B -> b, then X => s(a, b).
    /// Handles self-recursive structures like so: X => s(X) just returns s(X). However: X => s(X, Y), Y => 1 would resolve to s(X, 1).
    /// </summary>
    /// <param name="mapping">The mapping.</param>
    /// <param name="var">The variable to resolve.</param>
    /// <param name="doProhibitedValueResolution">Whether to resolve to prohibited values, see documentation of instance-based method.</param>
    /// <returns>A resolved <see cref="IVariableBinding"/>, or none if variable is not in mapping.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="var"/> is null,
    /// ..<paramref name="mapping"/> is null.</exception>
    public static IOption<IVariableBinding> Resolve(this VariableMapping mapping, Variable var, bool doProhibitedValueResolution)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        ArgumentNullException.ThrowIfNull(var, nameof(var));

        if (doProhibitedValueResolution)
        {
            return ToProhibResolver.Resolve(var, mapping);
        }
        else
        {
            return ToLastVariableResolver.Resolve(var, mapping);
        }
    }

    /// <summary>
    /// Applies a substitution to a term.
    /// </summary>
    /// <param name="mapping">The mapping.</param>
    /// <param name="term">The term to apply the substitution to.</param>
    /// <returns>The substituted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="mapping"/> is null,
    /// ..<paramref name="term"/> is null.</exception>
    public static ISimpleTerm ApplySubstitution(this VariableMapping mapping, ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        return Substituter.SubstituteTerm(term, mapping);
    }

    /// <summary>
    /// Applies a substitution to a structure and returns it as a structure..
    /// </summary>
    /// <param name="mapping">The mapping.</param>
    /// <param name="term">The structure to apply the substitution to.</param>
    /// <returns>The substituted structure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="mapping"/> is null,
    /// ..<paramref name="term"/> is null.</exception>
    public static Structure ApplySubstitution(this VariableMapping mapping, Structure term)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        return Substituter.SubstituteStructure(term, mapping);
    }
}
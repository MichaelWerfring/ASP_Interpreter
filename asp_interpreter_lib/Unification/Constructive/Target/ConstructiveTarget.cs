// <copyright file="ConstructiveTarget.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Target;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Unification.Constructive.Target.Builder;
using Asp_interpreter_lib.Util;
using System.Collections.Immutable;

/// <summary>
/// A class that holds a target for a constructive (dis)unification algorithm.
/// It consists of two terms and a mapping of prohibited values for each variable in the two terms.
/// This class does not do any checks for performance reasons:
/// Constructor should NEVER be called directly, always use <see cref="ConstructiveTargetBuilder"/>.
/// </summary>
public class ConstructiveTarget
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructiveTarget"/> class.
    /// </summary>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <param name="mapping">The mapping of prohibited values.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..left is null.
    /// ..right is null.
    /// ..mapping is null.</exception>
    public ConstructiveTarget(ISimpleTerm left, ISimpleTerm right, ImmutableDictionary<Variable, ProhibitedValuesBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(mapping);

        this.Left = left;
        this.Right = right;
        this.Mapping = mapping;
    }

    /// <summary>
    /// Gets the left term.
    /// </summary>
    public ISimpleTerm Left { get; }

    /// <summary>
    /// Gets the right term.
    /// </summary>
    public ISimpleTerm Right { get; }

    /// <summary>
    /// Gets the prohibited values mapping.
    /// </summary>
    public ImmutableDictionary<Variable, ProhibitedValuesBinding> Mapping { get; }

    /// <summary>
    /// Converts the target to a string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        return $"{{ {this.Left}, {this.Right}, [{this.Mapping.ToList().ListToString()}] }}";
    }
}
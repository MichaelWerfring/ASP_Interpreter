﻿// <copyright file="TermBindingTermBindingCase.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

/// <summary>
/// A class that represents a <see cref="TermBinding"/> - <see cref="TermBinding"/> case.
/// </summary>
public class TermBindingTermBindingCase : IBinaryVariableBindingCase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TermBindingTermBindingCase"/> class.
    /// </summary>
    /// <param name="left">The left binding.</param>
    /// <param name="right">The right binding.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="left"/> is null,
    /// ..<paramref name="right"/> is null.</exception>
    public TermBindingTermBindingCase(TermBinding left, TermBinding right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        this.Left = left;
        this.Right = right;
    }

    /// <summary>
    /// Gets the left binding.
    /// </summary>
    public TermBinding Left { get; }

    /// <summary>
    /// Gets the right binding.
    /// </summary>
    public TermBinding Right { get; }

    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="visitor"/> is null.</exception>
    public void Accept(IBinaryVariableBindingCaseVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        visitor.Visit(this);
    }

    /// <summary>
    /// Accepts a visitor that returns a value.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="visitor"/> is null.</exception>
    public T Accept<T>(IBinaryVariableBindingCaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        return visitor.Visit(this);
    }
}
// <copyright file="VariableVariableCase.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

/// <summary>
/// A class that represents a case where left is variable and right is variable.
/// </summary>
public class VariableVariableCase : IBinaryTermCase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VariableVariableCase"/> class.
    /// </summary>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// ..right is null.</exception>
    public VariableVariableCase(Variable left, Variable right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        this.Left = left;
        this.Right = right;
    }

    /// <summary>
    /// Gets the left term.
    /// </summary>
    public Variable Left { get; }

    /// <summary>
    /// Gets the right term.
    /// </summary>
    public Variable Right { get; }

    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="visitor"/> is null.</exception>
    public void Accept(IBinaryTermCaseVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        visitor.Visit(this);
    }

    /// <summary>
    /// Accepts a visitor that returns a value.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <typeparam name="T">The return value.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="visitor"/> is null.</exception>
    /// <returns>A value of the type <typeparamref name="T"/>.</returns>
    public T Accept<T>(IBinaryTermCaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        return visitor.Visit(this);
    }
}
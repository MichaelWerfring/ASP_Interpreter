// <copyright file="IntegerIntegerCase.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

/// <summary>
/// A class that represents a case where left and right are integer terms.
/// </summary>
public class IntegerIntegerCase : IBinaryTermCase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IntegerIntegerCase"/> class.
    /// </summary>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// ..right is null.</exception>
    public IntegerIntegerCase(Integer left, Integer right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        this.Left = left;
        this.Right = right;
    }

    /// <summary>
    /// Gets the left term.
    /// </summary>
    public Integer Left { get; }

    /// <summary>
    /// Gets the right term.
    /// </summary>
    public Integer Right { get; }

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
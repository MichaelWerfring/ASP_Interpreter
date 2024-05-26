// <copyright file="Integer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

/// <summary>
/// Represents an integer term.
/// </summary>
public class Integer : IStructure
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Integer"/> class.
    /// </summary>
    /// <param name="value">The integer value.</param>
    public Integer(int value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Gets the value of the integer term.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <exception cref="ArgumentNullException">Thrown if visitor is null.</exception>
    public void Accept(ISimpleTermVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        visitor.Visit(this);
    }

    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <exception cref="ArgumentNullException">Thrown if visitor is null.</exception>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Accept<T>(ISimpleTermVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        return visitor.Visit(this);
    }

    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="arguments">The extra arguments.</param>
    /// <typeparam name="TArgs">The arguments type.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..visitor is null.
    /// ..arguments is null.</exception>
    public void Accept<TArgs>(ISimpleTermArgsVisitor<TArgs> visitor, TArgs arguments)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        ArgumentNullException.ThrowIfNull(arguments);

        visitor.Visit(this, arguments);
    }

    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="arguments">The extra arguments.</param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <typeparam name="TArgs">The arguments type.</typeparam>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..visitor is null.
    /// ..arguments is null.</exception>
    public TResult Accept<TResult, TArgs>(ISimpleTermArgsVisitor<TResult, TArgs> visitor, TArgs arguments)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        ArgumentNullException.ThrowIfNull(arguments);

        return visitor.Visit(this, arguments);
    }

    /// <summary>
    /// Converts the object to a string representation.
    /// </summary>
    /// <returns>The object as a string representation.</returns>
    public override string ToString()
    {
        return this.Value.ToString();
    }
}
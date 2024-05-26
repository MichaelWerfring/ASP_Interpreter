// <copyright file="Variable.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

/// <summary>
/// A class that represents a variable term.
/// </summary>
public class Variable : ISimpleTerm
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Variable"/> class.
    /// </summary>
    /// <param name="identifier">The identifier of the variable.</param>
    /// <exception cref="ArgumentException">Thrown if identifier is null or whitespace.</exception>
    public Variable(string identifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        this.Identifier = identifier;
    }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    public string Identifier { get; }

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
    /// Converts the variable term to its string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        return this.Identifier;
    }
}
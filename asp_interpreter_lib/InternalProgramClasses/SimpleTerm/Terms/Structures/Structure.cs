// <copyright file="Structure.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

using System.Collections.Immutable;
using System.Text;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.Util;

/// <summary>
/// Represents a compound term, as well as a simple atom, depending on whether it has children.
/// </summary>
public class Structure : IStructure
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Structure"/> class.
    /// </summary>
    /// <param name="functor">The functor of the term.</param>
    /// <param name="children">The children of the term.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. functor is null.
    /// .. children is null.</exception>
    public Structure(string functor, IEnumerable<ISimpleTerm> children)
    {
        ArgumentException.ThrowIfNullOrEmpty(functor);
        ArgumentNullException.ThrowIfNull(children);

        this.Functor = functor;
        this.Children = children.ToImmutableList();
    }

    /// <summary>
    /// Gets the functor of the term.
    /// </summary>
    public string Functor { get; }

    /// <summary>
    /// Gets the children of the term.
    /// </summary>
    public IImmutableList<ISimpleTerm> Children { get; }

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
        StringBuilder stringBuilder = new();

        stringBuilder.Append(this.Functor);

        if (this.Children.Count > 0)
        {
            stringBuilder.Append('(');
            stringBuilder.Append(this.Children.ToList().ListToString());
            stringBuilder.Append(')');
        }

        return stringBuilder.ToString();
    }
}
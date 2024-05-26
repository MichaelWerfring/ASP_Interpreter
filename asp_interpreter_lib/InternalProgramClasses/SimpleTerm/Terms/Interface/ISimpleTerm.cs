// <copyright file="ISimpleTerm.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

/// <summary>
/// An interface representing a term.
/// </summary>
public interface ISimpleTerm
{
    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    public void Accept(ISimpleTermVisitor visitor);

    /// <summary>
    /// Accepts a visitor that returns a value.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Accept<T>(ISimpleTermVisitor<T> visitor);

    /// <summary>
    /// Accepts a visitor that comes with additional arguments.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="argument">The extra argument.</param>
    /// <typeparam name="TArgs">The argument type.</typeparam>
    public void Accept<TArgs>(ISimpleTermArgsVisitor<TArgs> visitor, TArgs argument);

    /// <summary>
    /// Accepts a visitor that returns a value and comes with additional arguments.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="argument">The extra argument.</param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <typeparam name="TArgs">The argument type.</typeparam>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    public TResult Accept<TResult, TArgs>(ISimpleTermArgsVisitor<TResult, TArgs> visitor, TArgs argument);

    /// <summary>
    /// Converts the term to a string representation.
    /// </summary>
    /// <returns>The term as a string representation.</returns>
    public abstract string ToString();
}
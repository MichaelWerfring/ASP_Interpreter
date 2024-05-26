// <copyright file="ISimpleTermVisitor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

/// <summary>
/// An interface for a term visitor.
/// </summary>
public interface ISimpleTermVisitor
{
    /// <summary>
    /// Visits a variable term.
    /// </summary>
    /// <param name="variableTerm">The term to visit.</param>
    void Visit(Variable variableTerm);

    /// <summary>
    /// Visits a structure term.
    /// </summary>
    /// <param name="structure">The term to visit.</param>
    void Visit(Structure structure);

    /// <summary>
    /// Visits a structure term.
    /// </summary>
    /// <param name="integer">The term to visit.</param>
    void Visit(Integer integer);
}

/// <summary>
/// An interface for a term visitor that returns a value.
/// </summary>
/// <typeparam name="T">The return type.</typeparam>
public interface ISimpleTermVisitor<T>
{
    /// <summary>
    /// Visits a variable term.
    /// </summary>
    /// <param name="variableTerm">The term to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    T Visit(Variable variableTerm);

    /// <summary>
    /// Visits a structure term.
    /// </summary>
    /// <param name="structure">The term to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    T Visit(Structure structure);

    /// <summary>
    /// Visits an integer term.
    /// </summary>
    /// <param name="integer">The term to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    T Visit(Integer integer);
}

/// <summary>
/// Represents visitor that comes with an additional argument.
/// </summary>
/// <typeparam name="TArgs">The type of the argument.</typeparam>
public interface ISimpleTermArgsVisitor<TArgs>
{
    /// <summary>
    /// Visits a variable term.
    /// </summary>
    /// <param name="variableTerm">The term to visit.</param>
    /// <param name="argument">The extra argument.</param>
    void Visit(Variable variableTerm, TArgs argument);

    /// <summary>
    /// Visits a structure term.
    /// </summary>
    /// <param name="structure">The term to visit.</param>
    /// <param name="argument">The extra argument.</param>
    void Visit(Structure structure, TArgs argument);

    /// <summary>
    /// Visits an integer term.
    /// </summary>
    /// <param name="integer">The term to visit.</param>
    /// <param name="argument">The extra argument.</param>
    void Visit(Integer integer, TArgs argument);
}

/// <summary>
/// Represents a visitor that comes with an additional argument and returns a value.
/// </summary>
/// <typeparam name="TResult">The result type.</typeparam>
/// <typeparam name="TArgs">The argument type.</typeparam>
public interface ISimpleTermArgsVisitor<TResult, TArgs>
{
    /// <summary>
    /// Visits a variable term.
    /// </summary>
    /// <param name="variableTerm">The term to visit.</param>
    /// <param name="argument">The extra argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    TResult Visit(Variable variableTerm, TArgs argument);

    /// <summary>
    /// Visits a structure term.
    /// </summary>
    /// <param name="structure">The term to visit.</param>
    /// <param name="argument">The extra argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    TResult Visit(Structure structure, TArgs argument);

    /// <summary>
    /// Visits an integer term.
    /// </summary>
    /// <param name="integer">The term to visit.</param>
    /// <param name="argument">The extra argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    TResult Visit(Integer integer, TArgs argument);
}
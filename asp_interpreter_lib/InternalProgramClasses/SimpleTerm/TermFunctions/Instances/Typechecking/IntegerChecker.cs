// <copyright file="IntegerChecker.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.Typechecking;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for type-safe conversion of a term into an integer term.
/// </summary>
internal class IntegerChecker : ISimpleTermVisitor<IOption<Integer>>
{
    /// <summary>
    /// Returns the term as an integer, or none.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The term as an integer, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<Integer> ReturnIntegerOrNone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    /// <summary>
    /// Visits the term to determine its type.
    /// </summary>
    /// <param name="term">The term to visit.</param>
    /// <returns>The term as an integer, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<Integer> Visit(Variable term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<Integer>();
    }

    /// <summary>
    /// Visits the term to determine its type.
    /// </summary>
    /// <param name="term">The term to visit.</param>
    /// <returns>The term as an integer, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<Integer> Visit(Structure term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<Integer>();
    }

    /// <summary>
    /// Visits the term to determine its type.
    /// </summary>
    /// <param name="term">The term to visit.</param>
    /// <returns>The term as an integer, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<Integer> Visit(Integer term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<Integer>(term);
    }
}
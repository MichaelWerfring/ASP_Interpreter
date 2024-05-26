// <copyright file="VariableChecker.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.Typechecking;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for type-safe conversion of a term into a variable term.
/// </summary>
internal class VariableChecker : ISimpleTermVisitor<IOption<Variable>>
{
    /// <summary>
    /// Returns the term as a variable, or none.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The term as a variable, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<Variable> ReturnVariableOrNone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    /// <summary>
    /// Visits the term to determine its type.
    /// </summary>
    /// <param name="term">The term to visit.</param>
    /// <returns>The term as a variable, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<Variable> Visit(Variable term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<Variable>(term);
    }

    /// <summary>
    /// Visits the term to determine its type.
    /// </summary>
    /// <param name="term">The term to visit.</param>
    /// <returns>The term as a variable, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<Variable> Visit(Structure term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<Variable>();
    }

    /// <summary>
    /// Visits the term to determine its type.
    /// </summary>
    /// <param name="term">The term to visit.</param>
    /// <returns>The term as a variable, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IOption<Variable> Visit(Integer term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<Variable>();
    }
}
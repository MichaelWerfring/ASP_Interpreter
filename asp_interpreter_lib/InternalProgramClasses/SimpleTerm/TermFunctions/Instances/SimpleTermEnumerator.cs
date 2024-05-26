// <copyright file="SimpleTermEnumerator.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

/// <summary>
/// Enumerator all the terms inside a term, including itself.
/// </summary>
public class SimpleTermEnumerator : ISimpleTermVisitor<IEnumerable<ISimpleTerm>>
{
    /// <summary>
    /// Enumerates the input term and all its children, if it has any.
    /// </summary>
    /// <param name="term">The term to enumerate.</param>
    /// <returns>An enumeration of terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IEnumerable<ISimpleTerm> Enumerate(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        foreach (var t in term.Accept(this))
        {
            yield return t;
        }
    }

    /// <summary>
    /// Visits the input term to enumerate it.
    /// </summary>
    /// <param name="variable">The term to enumerate.</param>
    /// <returns>An enumeration of terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IEnumerable<ISimpleTerm> Visit(Variable variable)
    {
        ArgumentNullException.ThrowIfNull(variable);

        yield return variable;
    }

    /// <summary>
    /// Visits the input term to enumerate it.
    /// </summary>
    /// <param name="structure">The term to enumerate.</param>
    /// <returns>An enumeration of terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IEnumerable<ISimpleTerm> Visit(Structure structure)
    {
        ArgumentNullException.ThrowIfNull(structure);

        yield return structure;

        foreach (var childTerm in structure.Children)
        {
            foreach (var term in childTerm.Accept(this))
            {
                yield return term;
            }
        }
    }

    /// <summary>
    /// Visits the input term to enumerate it.
    /// </summary>
    /// <param name="integer">The term to enumerate.</param>
    /// <returns>An enumeration of terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public IEnumerable<ISimpleTerm> Visit(Integer integer)
    {
        ArgumentNullException.ThrowIfNull(integer);

        yield return integer;
    }
}
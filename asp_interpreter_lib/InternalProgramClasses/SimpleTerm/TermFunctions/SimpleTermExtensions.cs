// <copyright file="SimpleTermExtensions.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using System.Collections.Immutable;

/// <summary>
/// A static class that contains extension methods for terms.
/// </summary>
public static class SimpleTermExtensions
{
    private static readonly SimpleTermEnumerator Flattener = new();
    private static readonly SimpleTermComparer Comparer = new();
    private static readonly SimpleTermContainsChecker ContainsChecker = new();
    private static readonly VariableSubstituter VariableSubstituter = new();

    /// <summary>
    /// Checks if term is equal to other.
    /// </summary>
    /// <param name="term">The term to check.</param>
    /// <param name="other">The other term.</param>
    /// <returns>A value indicating equality.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. other is null.</exception>
    public static bool IsEqualTo(this ISimpleTerm term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return Comparer.Compare(term, other) == 0;
    }

    /// <summary>
    /// Checks if a structure term is negated.
    /// </summary>
    /// <param name="term">The term to check.</param>
    /// <param name="functors">The functor table containing the naf string.</param>
    /// <returns>A value indicating negation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. functorTable is null.</exception>
    public static bool IsNegated(this Structure term, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(functors);

        if
        (
            term.Functor != functors.NegationAsFailure
            ||
            term.Children.Count != 1
            ||
            !TermFuncs.ReturnStructureOrNone(term.Children[0]).HasValue)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// CHecks if term contains other.
    /// </summary>
    /// <param name="term">The term to check for whether it contains other.</param>
    /// <param name="other">The term to check for whether it is inside term.</param>
    /// <returns>A value indicating whether term contains other.</returns>
    public static bool Contains(this ISimpleTerm term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return ContainsChecker.LeftContainsRight(term, other);
    }

    /// <summary>
    /// Compares two terms for their ordering.
    /// </summary>
    /// <param name="term">The left term.</param>
    /// <param name="other">The right term.</param>
    /// <returns>An integer indicating ordering.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. other is null.</exception>
    public static int Compare(this ISimpleTerm term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return Comparer.Compare(term, other);
    }

    /// <summary>
    /// Substitutes a term using a mapping.
    /// </summary>
    /// <param name="term">The term to substitute.</param>
    /// <param name="map">The substitution mapping.</param>
    /// <returns>The substituted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. map is null.</exception>
    public static ISimpleTerm Substitute(this ISimpleTerm term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return VariableSubstituter.Substitute(term, map);
    }

    /// <summary>
    /// Substitutes a term and returns it as a structure.
    /// </summary>
    /// <param name="term">The term to substitute.</param>
    /// <param name="map">The substitution mapping.</param>
    /// <returns>The substituted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. map is null.</exception>
    public static Structure Substitute(this Structure term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return VariableSubstituter.SubsituteStructure(term, map);
    }

    /// <summary>
    /// Negates a term.
    /// </summary>
    /// <param name="term">The term to negate.</param>
    /// <param name="functors">The functor table containing the naf identifier.</param>
    /// <returns>The negated term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. functorTable is null.</exception>
    public static Structure NegateTerm(this Structure term, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(functors);

        if
        (
            term.Functor != functors.NegationAsFailure
            ||
            term.Children.Count != 1)
        {
            return new Structure(functors.NegationAsFailure,[term]);
        }

        var innerStructMaybe = TermFuncs.ReturnStructureOrNone(term.Children[0]);

        if (!innerStructMaybe.HasValue)
        {
            return new Structure(functors.NegationAsFailure,[term]);
        }

        return innerStructMaybe.GetValueOrThrow();
    }

    /// <summary>
    /// Extracts the variables from a term and enumerates term.
    /// </summary>
    /// <param name="term">The term to extract variables from.</param>
    /// <returns>The variables from the term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public static IEnumerable<Variable> ExtractVariables(this ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Enumerate().OfType<Variable>().ToImmutableHashSet(TermFuncs.GetSingletonVariableComparer());
    }

    /// <summary>
    /// Enumerates all terms inside a term, including itself.
    /// </summary>
    /// <param name="term">The term to enumerate.</param>
    /// <returns>An enumeration of all terms inside the input term.</returns>
    public static IEnumerable<ISimpleTerm> Enumerate(this ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return Flattener.Enumerate(term);
    }
}
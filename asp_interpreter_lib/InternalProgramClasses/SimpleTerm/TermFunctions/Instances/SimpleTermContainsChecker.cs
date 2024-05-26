// <copyright file="SimpleTermContainsChecker.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

/// <summary>
/// A class for checking whether a term contains another term.
/// </summary>
public class SimpleTermContainsChecker : ISimpleTermArgsVisitor<bool, ISimpleTerm>
{
    /// <summary>
    /// Checks whether left contains right.
    /// </summary>
    /// <param name="term">The left term.</param>
    /// <param name="other">The term to check for whether it is inside the left term.</param>
    /// <returns>A value indicating whether other is in term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. other is null.</exception>
    public bool LeftContainsRight(ISimpleTerm term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return term.Accept(this, other);
    }

    /// <summary>
    /// Visits the left term with the right on as an additional argument,
    /// checking whether it contains the right one.
    /// </summary>
    /// <param name="term">The left term.</param>
    /// <param name="other">The term to check for whether it is inside the left term.</param>
    /// <returns>A value indicating whether other is in term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. other is null.</exception>
    public bool Visit(Structure term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        var areEqual = term.IsEqualTo(other);

        if (areEqual)
        {
            return true;
        }

        if (term.Children.Any(child => child.Accept(this, other)))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Visits the left term with the right on as an additional argument,
    /// checking whether it contains the right one.
    /// </summary>
    /// <param name="term">The left term.</param>
    /// <param name="other">The term to check for whether it is inside the left term.</param>
    /// <returns>A value indicating whether other is in term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. other is null.</exception>
    public bool Visit(Variable term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return term.IsEqualTo(other);
    }

    /// <summary>
    /// Visits the left term with the right on as an additional argument,
    /// checking whether it contains the right one.
    /// </summary>
    /// <param name="term">The left term.</param>
    /// <param name="other">The term to check for whether it is inside the left term.</param>
    /// <returns>A value indicating whether other is in term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. term is null.
    /// .. other is null.</exception>
    public bool Visit(Integer term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return term.IsEqualTo(other);
    }
}
// <copyright file="StructureReducer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class that tries to reduce two structures, meaning collapse them down to their children.
/// For example: s(1,2) and s(X, Y) would reduce, aswell as two integers 1 and 1, but s(1, 2) and g(X) would not.
/// </summary>
public class StructureReducer : ISimpleTermArgsVisitor<IOption<IEnumerable<(ISimpleTerm LeftChild, ISimpleTerm RightChild)>>, IStructure>
{
    /// <summary>
    /// Tries to reduce the term and returns a zipping of their children, or none.
    /// </summary>
    /// <param name="term">The left term.</param>
    /// <param name="other">The right term.</param>
    /// <returns>A zipping of the children of the input terms, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is null.</exception>
    public IOption<IEnumerable<(ISimpleTerm LeftChild, ISimpleTerm RightChild)>> TryReduce(IStructure term, IStructure other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return term.Accept(this, other);
    }

    /// <summary>
    /// Visits the left term and checks if it can reduce with the right term.
    /// </summary>
    /// <param name="term">The left term.</param>
    /// <param name="other">The right term.</param>
    /// <returns>A zipping of the children of the input terms, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is null.</exception>
    public IOption<IEnumerable<(ISimpleTerm LeftChild, ISimpleTerm RightChild)>> Visit(Structure term, IStructure other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        var otherAsStructureMaybe = TermFuncs.ReturnStructureOrNone(other);

        if (!otherAsStructureMaybe.HasValue)
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        Structure otherAsStructure = otherAsStructureMaybe.GetValueOrThrow();

        if (term.Functor != otherAsStructure.Functor)
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        if (term.Children.Count != otherAsStructure.Children.Count)
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        return new Some<IEnumerable<(ISimpleTerm, ISimpleTerm)>>(term.Children.Zip(otherAsStructure.Children));
    }

    /// <summary>
    /// Visits the left term and checks if it can reduce with the right term.
    /// </summary>
    /// <param name="term">The left term.</param>
    /// <param name="other">The right term.</param>
    /// <returns>A zipping of the children of the input terms, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is null.</exception>
    public IOption<IEnumerable<(ISimpleTerm LeftChild, ISimpleTerm RightChild)>> Visit(Integer term, IStructure other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        var otherAsIntegerMaybe = TermFuncs.ReturnIntegerOrNone(other);

        if (!otherAsIntegerMaybe.HasValue)
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        Integer otherAsInteger = otherAsIntegerMaybe.GetValueOrThrow();

        if (term.Value != otherAsInteger.Value)
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        return new Some<IEnumerable<(ISimpleTerm, ISimpleTerm)>>([]);
    }

    /// <summary>
    /// Visits the left term and checks if it can reduce with the right term.
    /// </summary>
    /// <param name="term">The left term.</param>
    /// <param name="other">The right term.</param>
    /// <returns>A zipping of the children of the input terms, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is null.</exception>
    public IOption<IEnumerable<(ISimpleTerm LeftChild, ISimpleTerm RightChild)>> Visit(Variable term, IStructure other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
    }
}
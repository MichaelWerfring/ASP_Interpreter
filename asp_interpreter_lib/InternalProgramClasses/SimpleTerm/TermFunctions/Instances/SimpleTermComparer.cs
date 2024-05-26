// <copyright file="SimpleTermComparer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

/// <summary>
/// A class for comparing two terms, based on the SWI-Prolog "Standard Order of Terms":
/// Variables smaller than Numbers smaller than Compounds.
/// Compounds : check arity, check functor, then check children from left to right.
/// </summary>
public class SimpleTermComparer : IComparer<ISimpleTerm>, IBinaryTermCaseVisitor<int>
{
    /// <summary>
    /// Compares two terms.
    /// </summary>
    /// <param name="x">The left term.</param>
    /// <param name="y">The right term.</param>
    /// <returns>An integer indicating ordering of the input terms.</returns>
    public int Compare(ISimpleTerm? x, ISimpleTerm? y)
    {
        if (x == null && y == null)
        {
            return 0;
        }

        if (x == null)
        {
            return -1;
        }

        if (y == null)
        {
            return 1;
        }

        return TermFuncs.DetermineCase(x, y).Accept(this);
    }

    /// <summary>
    /// Visits the two terms and determines their ordering.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <returns>The ordering of the input terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if case is null.</exception>
    public int Visit(IntegerIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return binaryCase.Left.Value.CompareTo(binaryCase.Right.Value);
    }

    /// <summary>
    /// Visits the two terms and determines their ordering.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <returns>The ordering of the input terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if case is null.</exception>
    public int Visit(IntegerStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return -1;
    }

    /// <summary>
    /// Visits the two terms and determines their ordering.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <returns>The ordering of the input terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if case is null.</exception>
    public int Visit(IntegerVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return 1;
    }

    /// <summary>
    /// Visits the two terms and determines their ordering.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <returns>The ordering of the input terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if case is null.</exception>
    public int Visit(StructureIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return 1;
    }

    /// <summary>
    /// Visits the two terms and determines their ordering.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <returns>The ordering of the input terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if case is null.</exception>
    public int Visit(StructureStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        var childCountComparison = binaryCase.Left.Children.Count.CompareTo(binaryCase.Right.Children.Count);

        if (childCountComparison != 0)
        {
            return childCountComparison;
        }

        var functorComparions = binaryCase.Left.Functor.CompareTo(binaryCase.Right.Functor);

        if (functorComparions != 0)
        {
            return functorComparions;
        }

        for (int i = 0; i < binaryCase.Left.Children.Count; i++)
        {
            var currentChildrenComparison = TermFuncs.DetermineCase(
                binaryCase.Left.Children.ElementAt(i), binaryCase.Right.Children.ElementAt(i))
                .Accept(this);

            if (currentChildrenComparison != 0)
            {
                return currentChildrenComparison;
            }
        }

        return 0;
    }

    /// <summary>
    /// Visits the two terms and determines their ordering.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <returns>The ordering of the input terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if case is null.</exception>
    public int Visit(StructureVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return 1;
    }

    /// <summary>
    /// Visits the two terms and determines their ordering.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <returns>The ordering of the input terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if case is null.</exception>
    public int Visit(VariableIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return -1;
    }

    /// <summary>
    /// Visits the two terms and determines their ordering.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <returns>The ordering of the input terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if case is null.</exception>
    public int Visit(VariableVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return binaryCase.Left.Identifier.CompareTo(binaryCase.Right.Identifier);
    }

    /// <summary>
    /// Visits the two terms and determines their ordering.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <returns>The ordering of the input terms.</returns>
    /// <exception cref="ArgumentNullException">Thrown if case is null.</exception>
    public int Visit(VariableStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return -1;
    }
}
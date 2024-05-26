// <copyright file="PostprocessingTermComparer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

/// <summary>
/// A class for comparing terms for sorting of terms during postprocessing.
/// </summary>
internal class PostprocessingTermComparer : IComparer<ISimpleTerm>, IBinaryTermCaseVisitor<int>
{
    /// <summary>
    /// Compares two terms for ordering.
    /// </summary>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <returns>An integer representing ordering.</returns>
    public int Compare(ISimpleTerm? left, ISimpleTerm? right)
    {
        if (left == null && right == null)
        {
            return 0;
        }

        if (left == null)
        {
            return -1;
        }

        if (right == null)
        {
            return 1;
        }

        var binaryCase = TermFuncs.DetermineCase(left, right);

        return binaryCase.Accept(this);
    }

    /// <summary>
    /// Visits a case and returns its ordering.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>An integer representing ordering.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public int Visit(IntegerIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return binaryCase.Left.Value.CompareTo(binaryCase.Right.Value);
    }

    /// <summary>
    /// Visits a case and returns its ordering.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>An integer representing ordering.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public int Visit(IntegerStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return -1;
    }

    /// <summary>
    /// Visits a case and returns its ordering.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>An integer representing ordering.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public int Visit(IntegerVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return 1;
    }

    /// <summary>
    /// Visits a case and returns its ordering.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>An integer representing ordering.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public int Visit(StructureIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return 1;
    }

    /// <summary>
    /// Visits a case and returns its ordering.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>An integer representing ordering.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public int Visit(StructureStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        var functorComparions = binaryCase.Left.Functor.CompareTo(binaryCase.Right.Functor);

        if (functorComparions != 0)
        {
            return functorComparions;
        }

        var childCountComparison = binaryCase.Left.Children.Count.CompareTo(binaryCase.Right.Children.Count);

        if (childCountComparison != 0)
        {
            return childCountComparison;
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
    /// Visits a case and returns its ordering.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>An integer representing ordering.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public int Visit(StructureVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return 1;
    }

    /// <summary>
    /// Visits a case and returns its ordering.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>An integer representing ordering.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public int Visit(VariableIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return -1;
    }

    /// <summary>
    /// Visits a case and returns its ordering.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>An integer representing ordering.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public int Visit(VariableVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return binaryCase.Left.Identifier.CompareTo(binaryCase.Right.Identifier);
    }

    /// <summary>
    /// Visits a case and returns its ordering.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>An integer representing ordering.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public int Visit(VariableStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return -1;
    }
}
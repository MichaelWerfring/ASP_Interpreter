// <copyright file="CHSPostprocessingComparer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;

/// <summary>
/// A class for comparing chs entries for sorting of terms during postprocessing.
/// </summary>
internal class CHSPostprocessingComparer : IComparer<CHSEntry>
{
    private FunctorTableRecord functors;

    private PostprocessingTermComparer comparer;

    /// <summary>
    /// Initializes a new instance of the <see cref="CHSPostprocessingComparer"/> class.
    /// </summary>
    /// <param name="functorMapping">The functor mapping for determining how a NaF looks like.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="functorMapping"/> is null.</exception>
    public CHSPostprocessingComparer(FunctorTableRecord functorMapping)
    {
        ArgumentNullException.ThrowIfNull(functorMapping, nameof(functorMapping));

        this.functors = functorMapping;
        this.comparer = new();
    }

    /// <summary>
    /// Compares two chs entries for ordering.
    /// </summary>
    /// <param name="left">The left entry.</param>
    /// <param name="right">The right entry.</param>
    /// <returns>An integer representing ordering.</returns>
    public int Compare(CHSEntry? left, CHSEntry? right)
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

        if (left.Term.IsNegated(this.functors) && right.Term.IsNegated(this.functors))
        {
            return this.comparer.Compare(left.Term.Children.ElementAt(0), right.Term.Children.ElementAt(0));
        }

        if (left.Term.IsNegated(this.functors))
        {
            return 1;
        }

        if (right.Term.IsNegated(this.functors))
        {
            return -1;
        }

        return this.comparer.Compare(left.Term, right.Term);
    }
}
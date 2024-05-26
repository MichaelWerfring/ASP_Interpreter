// <copyright file="CHSPostprocessingComparer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;

internal class CHSPostprocessingComparer : IComparer<CHSEntry>
{
    private FunctorTableRecord functors;

    private PostprocessingTermComparer comparer;

    public CHSPostprocessingComparer(FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));

        this.functors = functors;
        this.comparer = new();
    }

    public int Compare(CHSEntry? x, CHSEntry? y)
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

        if (x.Term.IsNegated(this.functors) && y.Term.IsNegated(this.functors))
        {
            return this.comparer.Compare(x.Term.Children.ElementAt(0), y.Term.Children.ElementAt(0));
        }

        if (x.Term.IsNegated(this.functors))
        {
            return 1;
        }

        if (y.Term.IsNegated(this.functors))
        {
            return -1;
        }

        return this.comparer.Compare(x.Term, y.Term);
    }
}
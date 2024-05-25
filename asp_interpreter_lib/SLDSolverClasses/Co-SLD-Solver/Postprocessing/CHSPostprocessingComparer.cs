using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

internal class CHSPostprocessingComparer : IComparer<CHSEntry>
{
    private FunctorTableRecord _functors;

    private PostprocessingTermComparer _comparer = new();

    public CHSPostprocessingComparer(FunctorTableRecord functors)
    {
        _functors = functors;
    }

    public int Compare(CHSEntry? x, CHSEntry? y)
    {
        if (x == null && y == null) return 0;

        if (x == null) return -1;

        if (y == null) return 1;

        if (x.Term.IsNegated(_functors) && y.Term.IsNegated(_functors))
        {
            return _comparer.Compare(x.Term.Children.ElementAt(0), y.Term.Children.ElementAt(0));
        }

        if (x.Term.IsNegated(_functors)) return 1;

        if (y.Term.IsNegated(_functors)) return -1;

        return _comparer.Compare(x.Term, y.Term);
    }
}

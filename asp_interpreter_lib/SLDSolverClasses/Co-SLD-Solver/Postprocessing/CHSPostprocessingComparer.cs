using asp_interpreter_lib.FunctorNaming;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

internal class CHSPostprocessingComparer : IComparer<CHSEntry>
{
    private FunctorTableRecord _functors;

    public CHSPostprocessingComparer(FunctorTableRecord functors)
    {
        _functors = functors;
    }

    public int Compare(CHSEntry? x, CHSEntry? y)
    {
        if (x == null && y == null) return 0;

        if (x == null) return -1;

        if (y == null) return 1;

        if (x.Term.Functor == _functors.NegationAsFailure && x.Term.Functor == _functors.NegationAsFailure)
        {
            return TermFuncs.GetSingletonTermComparer().Compare(x.Term.Children.ElementAt(0), y.Term.Children.ElementAt(0));
        }

        if (x.Term.Functor == _functors.NegationAsFailure) return 1;

        if (y.Term.Functor == _functors.NegationAsFailure) return -1;

        return TermFuncs.GetSingletonTermComparer().Compare(x.Term, y.Term);
    }
}

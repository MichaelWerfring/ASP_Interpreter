using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;

public class CHSEntryComparer : IComparer<CHSEntry>
{
    private readonly SimpleTermComparer _comparer = new SimpleTermComparer();

    public int Compare(CHSEntry? x, CHSEntry? y)
    {
        if (x == null && y == null) { return 0; }
        if (x == null) { return -1; }
        if (y == null) { return 1; }

        return _comparer.Compare(x.Term, y.Term);
    }
}

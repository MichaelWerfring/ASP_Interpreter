using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using System.Diagnostics.CodeAnalysis;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;

internal class CHSEntryComparer : IComparer<CHSEntry>, IEqualityComparer<CHSEntry>
{
    private readonly SimpleTermComparer _comparer = new();

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

        if(y == null)
        {
            return 1; 
        }

        return _comparer.Compare(x.Term, y.Term);
    }

    public bool Equals(CHSEntry? x, CHSEntry? y)
    {
        return Compare(x, y) == 0;
    }

    public int GetHashCode([DisallowNull] CHSEntry obj)
    {
        throw new NotImplementedException();
    }
}

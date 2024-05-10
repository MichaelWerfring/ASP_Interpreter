using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CoinductiveHypothesisSet
{
    public CoinductiveHypothesisSet()
    {
        Entries = ImmutableSortedSet.Create<CHSEntry>(new CHSEntryComparer());
    }

    public CoinductiveHypothesisSet(ImmutableSortedSet<CHSEntry> termSet)
    {
        ArgumentNullException.ThrowIfNull(termSet);
        if (termSet.KeyComparer is not CHSEntryComparer)
        {
            throw new ArgumentException("Must contain correct comparer.");
        }

        Entries = termSet;
    }

    public ImmutableSortedSet<CHSEntry> Entries { get; }
}

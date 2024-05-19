using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using Medallion.Collections;
using System.Collections;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CoinductiveHypothesisSet : IEnumerable<CHSEntry>
{
    private ImmutableLinkedList<CHSEntry> _entries;

    public CoinductiveHypothesisSet()
    {       
        _entries = [];
    }

    public CoinductiveHypothesisSet(ImmutableLinkedList<CHSEntry> termSet)
    {
        ArgumentNullException.ThrowIfNull(termSet);

        _entries = termSet;
    }

    public int Count => _entries.Count;

    public CoinductiveHypothesisSet Add(CHSEntry value)
    {
        return new CoinductiveHypothesisSet(_entries.Prepend(value));
    }

    public CoinductiveHypothesisSet Update(ISimpleTerm term, bool isFulfilled)
    {
        var newList = _entries.AsParallel().Select(entry =>
        {
            if (entry.Term.IsEqualTo(term))
            {
                return new CHSEntry(term, isFulfilled);
            }
            else
            {
                return entry;
            }
        }).ToImmutableLinkedList();

        return new CoinductiveHypothesisSet(newList);
    }

    public IEnumerator<CHSEntry> GetEnumerator()
    {
        return _entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _entries.GetEnumerator();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}

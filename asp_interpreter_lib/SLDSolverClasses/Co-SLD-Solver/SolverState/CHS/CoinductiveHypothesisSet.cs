using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using Asp_interpreter_lib.Util;
using Medallion.Collections;
using System.Collections;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CoinductiveHypothesisSet : IEnumerable<CHSEntry>
{
    private readonly ImmutableLinkedList<CHSEntry> _entries;

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

    public CoinductiveHypothesisSet Update(Structure term, bool isFulfilled)
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
        return _entries.Count > 0 ? $"{{{_entries.ToList().ListToString()}}}" : "Empty CHS";
    }
}

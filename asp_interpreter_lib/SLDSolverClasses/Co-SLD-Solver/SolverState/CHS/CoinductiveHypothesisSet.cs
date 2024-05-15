using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using System.Collections;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CoinductiveHypothesisSet : IImmutableList<CHSEntry>
{
    public CoinductiveHypothesisSet()
    {
        Entries = [];
    }

    public CoinductiveHypothesisSet(ImmutableList<CHSEntry> termSet)
    {
        ArgumentNullException.ThrowIfNull(termSet);

        Entries = termSet;
    }

    public CHSEntry this[int index] => Entries[index];

    public ImmutableList<CHSEntry> Entries { get; }

    public int Count => Entries.Count;

    public CoinductiveHypothesisSet Add(CHSEntry value)
    {
        return new CoinductiveHypothesisSet(Entries.Add(value));
    }

    public CoinductiveHypothesisSet AddRange(IEnumerable<CHSEntry> items)
    {
        return new CoinductiveHypothesisSet(Entries.AddRange(items));
    }

    public CoinductiveHypothesisSet Clear()
    {
        return [];
    }

    public IEnumerator<CHSEntry> GetEnumerator()
    {
        return Entries.GetEnumerator();
    }

    public int IndexOf(CHSEntry item, int index, int count, IEqualityComparer<CHSEntry>? equalityComparer)
    {
        return Entries.IndexOf(item, index, count, equalityComparer);
    }

    public CoinductiveHypothesisSet Insert(int index, CHSEntry element)
    {
        return new CoinductiveHypothesisSet(Entries.Insert(index, element));
    }

    public CoinductiveHypothesisSet InsertRange(int index, IEnumerable<CHSEntry> items)
    {
        return new CoinductiveHypothesisSet(Entries.InsertRange(index, items));
    }

    public int LastIndexOf(CHSEntry item, int index, int count, IEqualityComparer<CHSEntry>? equalityComparer)
    {
        return Entries.LastIndexOf(item, index, count, equalityComparer);
    }

    public CoinductiveHypothesisSet Remove(CHSEntry value, IEqualityComparer<CHSEntry>? equalityComparer)
    {
        return new CoinductiveHypothesisSet(Entries.Remove(value, equalityComparer));
    }

    public CoinductiveHypothesisSet RemoveAll(Predicate<CHSEntry> match)
    {
        return new CoinductiveHypothesisSet(Entries.RemoveAll(match));
    }

    public CoinductiveHypothesisSet RemoveAt(int index)
    {
        return new CoinductiveHypothesisSet(Entries.RemoveAt(index));
    }

    public CoinductiveHypothesisSet RemoveRange(IEnumerable<CHSEntry> items, IEqualityComparer<CHSEntry>? equalityComparer)
    {
        return new CoinductiveHypothesisSet(Entries.RemoveRange(items, equalityComparer));  
    }

    public CoinductiveHypothesisSet RemoveRange(int index, int count)
    {
        return new CoinductiveHypothesisSet(Entries.RemoveRange(index, count));
    }

    public CoinductiveHypothesisSet Replace(CHSEntry oldValue, CHSEntry newValue, IEqualityComparer<CHSEntry>? equalityComparer)
    {
        return new CoinductiveHypothesisSet(Entries.Replace(oldValue, newValue, equalityComparer));
    }

    public CoinductiveHypothesisSet SetItem(int index, CHSEntry value)
    {
        return new CoinductiveHypothesisSet(Entries.SetItem(index, value));
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.Add(CHSEntry value)
    {
        throw new NotImplementedException();
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.AddRange(IEnumerable<CHSEntry> items)
    {
        throw new NotImplementedException();
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.Clear()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.Insert(int index, CHSEntry element)
    {
        return Insert(index, element);
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.InsertRange(int index, IEnumerable<CHSEntry> items)
    {
        return InsertRange(index, items);
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.Remove(CHSEntry value, IEqualityComparer<CHSEntry>? equalityComparer)
    {
        return Remove(value, equalityComparer);
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.RemoveAll(Predicate<CHSEntry> match)
    {
        return RemoveAll(match);
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.RemoveAt(int index)
    {
        return RemoveAt(index);
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.RemoveRange(IEnumerable<CHSEntry> items, IEqualityComparer<CHSEntry>? equalityComparer)
    {
        return RemoveRange(items, equalityComparer);
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.RemoveRange(int index, int count)
    {
        return RemoveRange(index, count);
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.Replace(CHSEntry oldValue, CHSEntry newValue, IEqualityComparer<CHSEntry>? equalityComparer)
    {
        return Replace(oldValue, newValue, equalityComparer);
    }

    IImmutableList<CHSEntry> IImmutableList<CHSEntry>.SetItem(int index, CHSEntry value)
    {
        return SetItem(index, value);
    }
}

// <copyright file="CoinductiveHypothesisSet.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;
using Asp_interpreter_lib.Util;
using Medallion.Collections;
using System.Collections;

public class CoinductiveHypothesisSet : IEnumerable<CHSEntry>
{
    private readonly ImmutableLinkedList<CHSEntry> entries;

    public CoinductiveHypothesisSet()
    {
        this.entries = [];
    }

    public CoinductiveHypothesisSet(ImmutableLinkedList<CHSEntry> termSet)
    {
        ArgumentNullException.ThrowIfNull(termSet);

        this.entries = termSet;
    }

    public int Count => this.entries.Count;

    public CoinductiveHypothesisSet Add(CHSEntry value)
    {
        return new CoinductiveHypothesisSet(this.entries.Prepend(value));
    }

    public CoinductiveHypothesisSet Update(Structure term, bool isFulfilled)
    {
        var newList = this.entries.AsParallel().Select(entry =>
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
        return this.entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.entries.GetEnumerator();
    }

    public override string ToString()
    {
        return this.entries.Count > 0 ? $"{{{this.entries.ToList().ListToString()}}}" : "Empty CHS";
    }
}
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

/// <summary>
/// A class representing a coinductive hypothesis set.
/// </summary>
public class CoinductiveHypothesisSet : IEnumerable<CHSEntry>
{
    private readonly ImmutableLinkedList<CHSEntry> entries;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoinductiveHypothesisSet"/> class.
    /// </summary>
    public CoinductiveHypothesisSet()
    {
        this.entries = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CoinductiveHypothesisSet"/> class.
    /// </summary>
    /// <param name="entries">A list of chs entries.</param>
    public CoinductiveHypothesisSet(ImmutableLinkedList<CHSEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        this.entries = entries;
    }

    /// <summary>
    /// Gets the entry count.
    /// </summary>
    public int Count => this.entries.Count;

    /// <summary>
    /// Adds a new entry to the set.
    /// </summary>
    /// <param name="value">The entry to add.</param>
    /// <returns>A new set with the entry added.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
    public CoinductiveHypothesisSet Add(CHSEntry value)
    {
        return new CoinductiveHypothesisSet(this.entries.Prepend(value));
    }

    /// <summary>
    /// Updates an entry with a new goal success value.
    /// </summary>
    /// <param name="term">The term to update.</param>
    /// <param name="hasSucceeded">A value indicating goal success.</param>
    /// <returns>A new chs with that goal being updated.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="term"/> is null.</exception>
    public CoinductiveHypothesisSet Update(Structure term, bool hasSucceeded)
    {
        var newList = this.entries.AsParallel().Select(entry =>
        {
            if (entry.Term.IsEqualTo(term))
            {
                return new CHSEntry(term, hasSucceeded);
            }
            else
            {
                return entry;
            }
        }).ToImmutableLinkedList();

        return new CoinductiveHypothesisSet(newList);
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>An enumerator for the entries.</returns>
    public IEnumerator<CHSEntry> GetEnumerator()
    {
        return this.entries.GetEnumerator();
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>An enumerator for the entries.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.entries.GetEnumerator();
    }

    /// <summary>
    /// Converts the chs to a string representation.
    /// </summary>
    /// <returns>A string representation.</returns>
    public override string ToString()
    {
        return this.entries.Count > 0 ? $"{{{this.entries.ToList().ListToString()}}}" : "Empty CHS";
    }
}
// <copyright file="CHSEntry.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

/// <summary>
/// Represents an entry for a <see cref="CoinductiveHypothesisSet"/>.
/// Contains a term and a bool indicating whether the goal has already succeeded.
/// </summary>
public class CHSEntry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CHSEntry"/> class.
    /// </summary>
    /// <param name="term">The chs entry goal term.</param>
    /// <param name="hasSucceeded">A value indicating whether the goal has succeeded.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="term"/> is null.</exception>
    public CHSEntry(Structure term, bool hasSucceeded)
    {
        ArgumentNullException.ThrowIfNull(term);

        this.Term = term;
        this.HasSucceded = hasSucceeded;
    }

    /// <summary>
    /// Gets the goal term.
    /// </summary>
    public Structure Term { get; }

    /// <summary>
    /// Gets a value indicating whether the term has succeeded.
    /// </summary>
    public bool HasSucceded { get; }

    /// <summary>
    /// Converts the chs entry to a string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        return $"[{this.Term}:{this.HasSucceded}]";
    }
}
//-----------------------------------------------------------------------
// <copyright file="RenamingResult.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

/// <summary>
/// A class that represents a renaming result by a <see cref="ClauseVariableRenamer"/>.
/// </summary>
public class RenamingResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenamingResult"/> class.
    /// </summary>
    /// <param name="clause">The renamed clause.</param>
    /// <param name="nextInternalIndex">The next variable index.</param>
    /// <exception cref="ArgumentNullException">Thrown if clause is null.</exception>
    public RenamingResult(IEnumerable<Structure> clause, int nextInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        this.RenamedClause = clause;
        this.NextInternalIndex = nextInternalIndex;
    }

    /// <summary>
    /// Gets the renamed clause.
    /// </summary>
    public IEnumerable<Structure> RenamedClause { get; }

    /// <summary>
    /// Gets the next variable index.
    /// </summary>
    public int NextInternalIndex { get; }
}
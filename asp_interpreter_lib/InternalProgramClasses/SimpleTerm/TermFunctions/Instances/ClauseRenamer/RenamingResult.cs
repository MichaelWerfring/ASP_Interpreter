//-----------------------------------------------------------------------
// <copyright file="RenamingResult.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;

public class RenamingResult
{
    public RenamingResult(IEnumerable<Structure> clause, int nextInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        this.RenamedClause = clause;
        this.NextInternalIndex = nextInternalIndex;
    }

    public IEnumerable<Structure> RenamedClause { get; }

    public int NextInternalIndex { get; }
}
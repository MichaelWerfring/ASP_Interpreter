// <copyright file="CHSEntry.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

public class CHSEntry
{
    public CHSEntry(Structure term, bool hasSucceeded)
    {
        ArgumentNullException.ThrowIfNull(term);

        Term = term;
        HasSucceded = hasSucceeded;
    }

    public Structure Term { get; }

    public bool HasSucceded { get; }

    public override string ToString()
    {
        return $"[{Term}:{HasSucceded}]";
    }
}

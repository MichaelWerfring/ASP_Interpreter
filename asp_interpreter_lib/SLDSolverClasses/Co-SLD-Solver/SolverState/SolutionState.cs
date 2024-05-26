// <copyright file="SolutionState.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Text;

/// <summary>
/// Represents the current solution state of a coinductive solver.
/// </summary>
public class SolutionState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionState"/> class.
    /// </summary>
    /// <param name="callstack">The current callstack.</param>
    /// <param name="chs">The current chs.</param>
    /// <param name="mapping">The current mapping.</param>
    /// <param name="nextInternalIndex">The next internal variable index.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="callstack"/> is null,
    /// ..<paramref name="chs"/> is null,
    /// <paramref name="mapping"/> is null.</exception>
    public SolutionState(
        CallStack callstack,
        CoinductiveHypothesisSet chs,
        VariableMapping mapping,
        int nextInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(callstack, nameof(callstack));
        ArgumentNullException.ThrowIfNull(chs, nameof(chs));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        this.Callstack = callstack;
        this.CHS = chs;
        this.Mapping = mapping;
        this.NextInternalVariableIndex = nextInternalIndex;
    }

    /// <summary>
    /// Gets the current callstack.
    /// </summary>
    public CallStack Callstack { get; }

    /// <summary>
    /// Gets the current chs.
    /// </summary>
    public CoinductiveHypothesisSet CHS { get; }

    /// <summary>
    /// Gets the current mapping.
    /// </summary>
    public VariableMapping Mapping { get; }

    /// <summary>
    /// Gets the next internal variable index.
    /// </summary>
    public int NextInternalVariableIndex { get; }

    /// <summary>
    /// Converts the solution state to a string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("Callstack:");
        sb.AppendLine(this.Callstack.ToString());

        sb.AppendLine("CHS:");
        sb.AppendLine(this.CHS.ToString());

        sb.AppendLine("VariableMapping:");
        sb.AppendLine(this.Mapping.ToString());

        sb.AppendLine("NextVar");
        sb.AppendLine(this.NextInternalVariableIndex.ToString());

        return sb.ToString();
    }
}
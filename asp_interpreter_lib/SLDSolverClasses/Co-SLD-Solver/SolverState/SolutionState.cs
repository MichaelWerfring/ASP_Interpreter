// <copyright file="SolutionState.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Text;

public class SolutionState
{
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

    public CallStack Callstack { get; }

    public CoinductiveHypothesisSet CHS { get; }

    public VariableMapping Mapping { get; }

    public int NextInternalVariableIndex { get; }

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
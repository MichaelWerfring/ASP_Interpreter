using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Text;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

public class SolutionState
{
    public SolutionState
    (
        CallStack callstack,
        CoinductiveHypothesisSet chs,
        VariableMapping mapping,
        int nextInternalIndex
    )
    {
        ArgumentNullException.ThrowIfNull(callstack, nameof(callstack));
        ArgumentNullException.ThrowIfNull(chs, nameof(chs));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        Callstack = callstack;
        CHS = chs;
        Mapping = mapping;
        NextInternalVariableIndex = nextInternalIndex;
    }

    public CallStack Callstack { get; }

    public CoinductiveHypothesisSet CHS { get; }

    public VariableMapping Mapping { get; }

    public int NextInternalVariableIndex { get; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("Callstack:");
        sb.AppendLine(Callstack.ToString());

        sb.AppendLine("CHS:");
        sb.AppendLine(CHS.ToString());

        sb.AppendLine("VariableMapping:");
        sb.AppendLine(Mapping.ToString());

        sb.AppendLine("NextVar");
        sb.AppendLine(NextInternalVariableIndex.ToString());

        return sb.ToString();
    }
}

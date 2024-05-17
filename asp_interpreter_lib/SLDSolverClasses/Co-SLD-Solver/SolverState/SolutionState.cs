using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

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
}

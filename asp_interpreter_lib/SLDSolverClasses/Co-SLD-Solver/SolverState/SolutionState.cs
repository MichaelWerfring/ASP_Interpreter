using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

public class SolutionState
{
    public SolutionState
    (
        CallStack currentStack,
        CoinductiveHypothesisSet currentSet,
        VariableMapping currentMapping,
        int nextInternalIndex
    )
    {
        ArgumentNullException.ThrowIfNull(currentStack, nameof(currentStack));
        ArgumentNullException.ThrowIfNull(currentSet, nameof(currentSet));
        ArgumentNullException.ThrowIfNull(currentMapping, nameof(currentMapping));

        CurrentStack = currentStack;
        CurrentSet = currentSet;
        CurrentMapping = currentMapping;
        NextInternalVariableIndex = nextInternalIndex;
    }

    public CallStack CurrentStack { get; }

    public CoinductiveHypothesisSet CurrentSet { get; }

    public VariableMapping CurrentMapping { get; }

    public int NextInternalVariableIndex { get; }
}

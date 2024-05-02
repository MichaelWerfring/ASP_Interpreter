
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CoSldSolverState
{
    public CoSldSolverState
    (
        CallStack currentStack,
        CoinductiveHypothesisSet currentSet,
        VariableMapping currentMapping,
        IEnumerable<ISimpleTerm> currentGoals
    )
    {
        ArgumentNullException.ThrowIfNull(currentStack, nameof(currentStack));
        ArgumentNullException.ThrowIfNull(currentSet, nameof(currentSet));
        ArgumentNullException.ThrowIfNull(currentMapping, nameof(currentMapping));
        ArgumentNullException.ThrowIfNull(currentGoals, nameof(currentGoals));

        CurrentStack = currentStack;
        CurrentSet = currentSet;
        CurrentMapping = currentMapping;
        CurrentGoals = currentGoals;
    }

    public CallStack CurrentStack { get; }

    public CoinductiveHypothesisSet CurrentSet { get; }

    public VariableMapping CurrentMapping { get; }

    public IEnumerable<ISimpleTerm> CurrentGoals { get; }
}

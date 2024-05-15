using asp_interpreter_lib.Unification.Constructive.Disunification;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class DisunificationGoal : ICoSLDGoal
{
    private readonly SolverStateUpdater _updater = new();

    private readonly ConstructiveTarget _target;

    private readonly IConstructiveDisunificationAlgorithm _algorithm;

    private readonly SolutionState _solutionState; 

    public DisunificationGoal
    (
        ConstructiveTarget target,
        IConstructiveDisunificationAlgorithm algorithm,
        SolutionState solutionState
    )
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));

        _target = target;
        _algorithm = algorithm;
        _solutionState = solutionState;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        var disunificationsEither = _algorithm.Disunify(_target);

        if (!disunificationsEither.IsRight)
        {
            yield break;
        }

        var disunificationResults = disunificationsEither.GetRightOrThrow();

        foreach (VariableMapping result in disunificationResults)
        {
            CoinductiveHypothesisSet updatedCHS = _updater.UpdateCHS(_solutionState.CHS, result);

            CallStack updatedCallstack = _updater.UpdateCallstack(_solutionState.Callstack, result);

            yield return new GoalSolution
            (
                updatedCHS,
                result,
                updatedCallstack, 
                _solutionState.NextInternalVariableIndex
            );
        }
    }
}

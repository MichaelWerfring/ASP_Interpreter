using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class UnificationGoal : ICoSLDGoal
{
    private readonly SolverStateUpdater _updater;
    private readonly ConstructiveTarget _target;
    private readonly IConstructiveUnificationAlgorithm _algorithm;
    private readonly SolutionState _solutionState;
    private readonly ILogger _logger;

    public UnificationGoal
    (
        SolverStateUpdater updater,
        ConstructiveTarget target,
        IConstructiveUnificationAlgorithm algorithm,
        SolutionState solutionState,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(updater, nameof(updater));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _updater = updater;
        _target = target;
        _algorithm = algorithm;
        _solutionState = solutionState;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        _logger.LogInfo($"Attempting to solve unification goal: {_target}");
        _logger.LogTrace($"Input state is: {_solutionState}");

        var unificationMaybe = _algorithm.Unify(_target);
        if (!unificationMaybe.HasValue)
        {
            yield break;
        }

        _logger.LogInfo($"Solved unification goal: {_target}");

        var unifyingMapping = unificationMaybe.GetValueOrThrow();
        _logger.LogTrace($"Unifying mapping is {unifyingMapping}");

        var updatedMapping = _solutionState.Mapping.Update(unifyingMapping).GetValueOrThrow();
        _logger.LogTrace($"Updated mapping is {updatedMapping}");

        var flattenedMapping = updatedMapping.Flatten();
        _logger.LogTrace($"Flattened mapping is {updatedMapping}");

        CoinductiveHypothesisSet updatedCHS = _updater.UpdateCHS(_solutionState.CHS, flattenedMapping);
        _logger.LogTrace($"Updated CHS is {updatedCHS}");

        CallStack updatedCallstack = _updater.UpdateCallstack(_solutionState.Callstack, flattenedMapping);
        _logger.LogTrace($"Updated callstack is {updatedCallstack}");


        yield return new GoalSolution       
        (
            updatedCHS,
            flattenedMapping,
            updatedCallstack,
            _solutionState.NextInternalVariableIndex
        );
    }
}

using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class UnificationGoal : ICoSLDGoal
{
    private readonly SolverStateUpdater _updater = new();

    private readonly ConstructiveTarget _target;

    private readonly IConstructiveUnificationAlgorithm _algorithm;

    private readonly SolutionState _solutionState;

    private readonly ILogger _logger;

    public UnificationGoal
    (
        ConstructiveTarget target,
        IConstructiveUnificationAlgorithm algorithm,
        SolutionState solutionState,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

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

        var unifyingMapping = unificationMaybe.GetValueOrThrow();
        _logger.LogDebug($"Unifying mapping is {unifyingMapping}");

        var updatedMapping = _solutionState.Mapping.Update(unifyingMapping).GetValueOrThrow();
        _logger.LogDebug($"Updated mapping is {updatedMapping}");

        var flattenedMapping = updatedMapping.Flatten();
        _logger.LogDebug($"Flattened mapping is {updatedMapping}");

        CoinductiveHypothesisSet updatedCHS = _updater.UpdateCHS(_solutionState.CHS, flattenedMapping);

        CallStack updatedCallstack = _updater.UpdateCallstack(_solutionState.Callstack, flattenedMapping);

        yield return new GoalSolution       
        (
            updatedCHS,
            flattenedMapping,
            updatedCallstack,
            _solutionState.NextInternalVariableIndex
        );
    }
}

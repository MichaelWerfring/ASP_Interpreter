using asp_interpreter_lib.Unification.Constructive.Disunification;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Util.ErrorHandling.Either;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class DisunificationGoal : ICoSLDGoal
{
    private readonly SolverStateUpdater _updater = new();

    private readonly ConstructiveTarget _target;

    private readonly IConstructiveDisunificationAlgorithm _algorithm;

    private readonly SolutionState _inputState;

    private readonly ILogger _logger;

    public DisunificationGoal
    (
        ConstructiveTarget target,
        IConstructiveDisunificationAlgorithm algorithm,
        SolutionState state,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _target = target;
        _algorithm = algorithm;
        _inputState = state;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        _logger.LogInfo($"Attempting to solve disunification goal: {_target}");
        _logger.LogTrace($"Input state is: {_inputState}");

        var disunificationsEither = _algorithm.Disunify(_target);

        if (!disunificationsEither.IsRight)
        {
            yield break;
        }

        foreach (VariableMapping disunifyingMapping in disunificationsEither.GetRightOrThrow())
        {
            _logger.LogDebug($"Disunifying mapping is {disunifyingMapping}");

            var updatedMapping = _inputState.Mapping.Update(disunifyingMapping).GetValueOrThrow();
            _logger.LogDebug($"Updated mapping is {updatedMapping}");

            var flattenedMapping = updatedMapping.Flatten();
            _logger.LogDebug($"Flattened mapping is {updatedMapping}");

            CoinductiveHypothesisSet updatedCHS = _updater.UpdateCHS(_inputState.CHS, flattenedMapping);

            CallStack updatedCallstack = _updater.UpdateCallstack(_inputState.Callstack, flattenedMapping);

            yield return new GoalSolution
            (
                updatedCHS,
                flattenedMapping,
                updatedCallstack, 
                _inputState.NextInternalVariableIndex
            );
        }
    }
}

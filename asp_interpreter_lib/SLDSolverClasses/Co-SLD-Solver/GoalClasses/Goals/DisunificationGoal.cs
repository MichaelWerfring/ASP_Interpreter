using asp_interpreter_lib.Unification.Constructive.Disunification;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Util.ErrorHandling.Either;
using asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class DisunificationGoal : ICoSLDGoal
{
    private readonly SolverStateUpdater _updater;
    private readonly ConstructiveTarget _target;

    private readonly IConstructiveDisunificationAlgorithm _algorithm;

    private readonly SolutionState _inputState;

    private readonly ILogger _logger;

    public DisunificationGoal
    (
        SolverStateUpdater updater,
        ConstructiveTarget target,
        IConstructiveDisunificationAlgorithm algorithm,
        SolutionState state,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(updater, nameof(updater));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _updater = updater;
        _target = target;
        _algorithm = algorithm;
        _inputState = state;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        _logger.LogInfo($"Attempting to solve disunification goal: {_target}");
        _logger.LogTrace($"Input state is: {_inputState}");

        IEither<DisunificationException, IEnumerable<VariableMapping>> disunificationsResult = _algorithm.Disunify(_target);

        if (!disunificationsResult.IsRight)
        {
            yield break;
        }

        foreach (VariableMapping disunifyingMapping in disunificationsResult.GetRightOrThrow())
        {
            _logger.LogInfo($"Solved disunification goal: {_target} with {disunifyingMapping}");

            _logger.LogTrace($"Disunifying mapping is {disunifyingMapping}");

            VariableMapping updatedMapping = _inputState.Mapping.Update(disunifyingMapping).GetValueOrThrow();
            _logger.LogTrace($"Updated mapping is {updatedMapping}");

            VariableMapping flattenedMapping = updatedMapping.Flatten();
            _logger.LogTrace($"Flattened mapping is {updatedMapping}");

            CoinductiveHypothesisSet updatedCHS = _updater.UpdateCHS(_inputState.CHS, flattenedMapping);
            _logger.LogTrace($"updated CHS is {updatedCHS}");

            CallStack updatedCallstack = _updater.UpdateCallstack(_inputState.Callstack, flattenedMapping);
            _logger.LogTrace($"updated callstck is {updatedCallstack}");

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

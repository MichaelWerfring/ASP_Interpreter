using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target.Builder;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

internal class ArithmeticEvaluationGoal : ICoSLDGoal, ISimpleTermArgsVisitor<IOption<GoalSolution>, int>
{
    private readonly SolverStateUpdater _updater;
    private readonly ArithmeticEvaluator _evaluator;
    private readonly ISimpleTerm _left;
    private readonly ISimpleTerm _right;
    private readonly SolutionState _state;
    private readonly IConstructiveUnificationAlgorithm _algorithm;
    private readonly ILogger _logger;

    public ArithmeticEvaluationGoal
    (
        SolverStateUpdater updater,
        ArithmeticEvaluator evaluator,
        ISimpleTerm left,
        ISimpleTerm right,
        SolutionState state,
        IConstructiveUnificationAlgorithm algorithm,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(updater);
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        _updater = updater;
        _evaluator = evaluator;
        _left = left;
        _right = right;
        _state = state;
        _algorithm = algorithm;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        _logger.LogInfo($"Attempting to solve arithmetic evaluation goal: {_left}, {_right}");
        _logger.LogTrace($"Input state is: {_state}");

        IOption<int> rightEvalMaybe = _evaluator.Evaluate( _right );

        int rightEval;
        try
        {
            rightEval = rightEvalMaybe.GetValueOrThrow();
        }
        catch
        {
            yield break;
        }

        var solutionMaybe = _left.Accept(this, rightEval);    
        if (!solutionMaybe.HasValue)
        {
            yield break;
        }

        _logger.LogInfo($"Solved arithmetic evaluation goal: {_left}, {_right}");

        yield return solutionMaybe.GetValueOrThrow();
    }

    public IOption<GoalSolution> Visit(Variable var, int integer)
    {
        ArgumentNullException.ThrowIfNull(var);

        var targetEither = ConstructiveTargetBuilder.Build(var, new Integer(integer), _state.Mapping);
        if(!targetEither.IsRight)
        {
            _logger.LogError(targetEither.GetLeftOrThrow().Message);
            throw new ArgumentException(nameof(_state));
        }

        var target = targetEither.GetRightOrThrow();

        IOption<VariableMapping> unificationResult = _algorithm.Unify(target);
        if (!unificationResult.HasValue)
        {
            return new None<GoalSolution>();
        }

        VariableMapping unification = unificationResult.GetValueOrThrow();
        _logger.LogTrace($"Unifying mapping is {unification}");

        var updatedMapping = _state.Mapping.Update(unification).GetValueOrThrow();
        _logger.LogTrace($"Updated mapping is {updatedMapping}");

        var flattenedMapping = updatedMapping.Flatten();
        _logger.LogTrace($"Flattened mapping is {flattenedMapping}");

        CoinductiveHypothesisSet updatedCHS = _updater.UpdateCHS(_state.CHS, flattenedMapping);
        _logger.LogTrace($"Updated CHS is {updatedCHS}");

        CallStack updatedStack = _updater.UpdateCallstack(_state.Callstack, flattenedMapping);
        _logger.LogTrace($"Updated callstack is {updatedStack}");

        return new Some<GoalSolution>
        (
            new GoalSolution
            (
                updatedCHS,
                flattenedMapping,
                updatedStack,
                _state.NextInternalVariableIndex
            )
        );
    }

    public IOption<GoalSolution> Visit(Structure _, int __)
    {
        ArgumentNullException.ThrowIfNull(_);

        return new None<GoalSolution>();
    }

    public IOption<GoalSolution> Visit(Integer integer, int rightEval)
    {
        ArgumentNullException.ThrowIfNull(integer);

        if (integer.Value != rightEval)
        {
            return new None<GoalSolution>();
        }

        return new Some<GoalSolution>
        (
            new GoalSolution
            (
                _state.CHS,
                _state.Mapping,
                _state.Callstack,
                _state.NextInternalVariableIndex
            )
        );
    }
}

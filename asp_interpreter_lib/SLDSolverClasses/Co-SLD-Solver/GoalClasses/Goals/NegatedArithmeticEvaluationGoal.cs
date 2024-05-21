using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Disunification;
using asp_interpreter_lib.Unification.Constructive.Target.Builder;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals;

internal class NegatedArithmeticEvaluationGoal : ICoSLDGoal, ISimpleTermArgsVisitor<IOption<GoalSolution>, int>
{
    private readonly ArithmeticEvaluator _evaluator;

    private readonly ISimpleTerm _left;

    private readonly ISimpleTerm _right;

    private readonly SolutionState _state;

    private readonly IConstructiveDisunificationAlgorithm _algorithm;

    private readonly ILogger _logger;

    public NegatedArithmeticEvaluationGoal
    (
        ArithmeticEvaluator evaluator,
        ISimpleTerm left,
        ISimpleTerm right,
        SolutionState state,
        IConstructiveDisunificationAlgorithm algorithm,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        _evaluator = evaluator;
        _left = left;
        _right = right;
        _state = state;
        _algorithm = algorithm;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        _logger.LogInfo($"Attempting to solve negated arithmetic evaluation goal: {_left}, {_right}");
        _logger.LogTrace($"Input state is: {_state}");

        IOption<int> rightEvalMaybe = _evaluator.Evaluate(_right);

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

        yield return solutionMaybe.GetValueOrThrow();
    }

    public IOption<GoalSolution> Visit(Variable var, int integer)
    {
        var targetEither = ConstructiveTargetBuilder.Build(var, new Integer(integer), _state.Mapping);

        if (!targetEither.IsRight)
        {
            _logger.LogError(targetEither.GetLeftOrThrow().Message);
            throw new ArgumentException(nameof(_state));
        }

        var target = targetEither.GetRightOrThrow();

        var resultEither  =_algorithm.Disunify(target);

        if (!resultEither.IsRight)
        {
            return new None<GoalSolution>();
        }

        var disunifyingMapping = resultEither.GetRightOrThrow().First();

        _logger.LogDebug($"Disunifying mapping is {disunifyingMapping}");

        var newMapping = _state.Mapping.Update(resultEither.GetRightOrThrow().First()).GetValueOrThrow();
        _logger.LogDebug($"New mapping is {newMapping}");

        return new Some<GoalSolution>
        (
            new GoalSolution
            (
                _state.CHS,
                newMapping,
                _state.Callstack,
                _state.NextInternalVariableIndex
            )
        );
    }

    public IOption<GoalSolution> Visit(Structure _, int __)
    {
        return new None<GoalSolution>();
    }

    public IOption<GoalSolution> Visit(Integer integer, int rightEval)
    {
        if (integer.Value == rightEval)
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

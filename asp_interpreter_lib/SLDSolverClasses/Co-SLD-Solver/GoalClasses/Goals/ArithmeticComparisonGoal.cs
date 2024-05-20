using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.Comparison;

public class ArithmeticComparisonGoal : ICoSLDGoal
{
    private readonly ArithmeticEvaluator _evaluator;

    private readonly ISimpleTerm _left;

    private readonly ISimpleTerm _right;

    private readonly Func<int, int, bool> _predicate;

    private readonly SolutionState _inputstate;

    private readonly ILogger _logger;

    public ArithmeticComparisonGoal
    (
        ArithmeticEvaluator evaluator,
        ISimpleTerm left,
        ISimpleTerm right,
        Func<int, int, bool> predicate,
        SolutionState solutionState,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(solutionState);
        ArgumentNullException.ThrowIfNull(logger);

        _evaluator = evaluator;
        _left = left;
        _right = right;
        _predicate = predicate;
        _inputstate = solutionState;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        _logger.LogInfo($"Attempting to solve arithmetic comparison goal: {_left}, {_right}");
        _logger.LogTrace($"Input state is: {_inputstate}");

        Integer leftInteger;
        try
        {
            leftInteger = (Integer)_left;
        }
        catch
        {
            yield break;
        }

        var rightEvaluationMaybe = _evaluator.Evaluate(_right);

        if (!rightEvaluationMaybe.HasValue)
        {
            yield break;
        }

        var b =_predicate.Invoke(5, 0);
       
        if (!_predicate(leftInteger.Value, rightEvaluationMaybe.GetValueOrThrow()))
        {
            yield break;
        }

        yield return new GoalSolution
        (
            _inputstate.CHS, 
            _inputstate.Mapping, 
            _inputstate.Callstack,
            _inputstate.NextInternalVariableIndex
        );
    }
}

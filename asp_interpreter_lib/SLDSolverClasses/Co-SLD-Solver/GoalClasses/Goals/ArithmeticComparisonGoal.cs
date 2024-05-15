using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.Comparison;

public class ArithmeticComparisonGoal : ICoSLDGoal
{
    private readonly ArithmeticEvaluator _evaluator;

    private readonly ISimpleTerm _left;

    private readonly ISimpleTerm _right;

    private readonly Func<int, int, bool> _predicate;

    private readonly SolutionState _solutionState;

    public ArithmeticComparisonGoal
    (
        ArithmeticEvaluator evaluator,
        ISimpleTerm left,
        ISimpleTerm right,
        Func<int, int, bool> predicate,
        SolutionState solutionState
    )
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(solutionState);

        _evaluator = evaluator;
        _left = left;
        _right = right;
        _predicate = predicate;
        _solutionState = solutionState;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
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

        if (!_predicate(leftInteger.Value, rightEvaluationMaybe.GetValueOrThrow()))
        {
            yield break;
        }

        yield return new GoalSolution
        (
            _solutionState.CHS, 
            _solutionState.Mapping, 
            _solutionState.Callstack,
            _solutionState.NextInternalVariableIndex
        );
    }
}

using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.Comparison;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.GoalBuilders;

internal class ArithmeticComparisonGoalBuilder : IGoalBuilder
{
    private readonly Func<int, int, bool> _predicate;
    private readonly ArithmeticEvaluator _evaluator;
    private readonly ILogger _logger;

    public ArithmeticComparisonGoalBuilder(Func<int, int, bool> predicate, ArithmeticEvaluator evaluator, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(logger);

        _predicate = predicate;
        _evaluator = evaluator;
        _logger = logger;
    }

    public ICoSLDGoal BuildGoal(Structure goalTerm, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(goalTerm);
        ArgumentNullException.ThrowIfNull(state);

        if (goalTerm.Children.Count != 2)
        {
            throw new ArgumentException("Goal must contain a structure term with two children.", nameof(goalTerm)); 
        }

        return new ArithmeticComparisonGoal
        (
            _evaluator,
            goalTerm.Children.ElementAt(0),
            goalTerm.Children.ElementAt(1),
            _predicate,
            state,
            _logger
        );
    }
}

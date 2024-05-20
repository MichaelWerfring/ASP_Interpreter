using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.Comparison;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.GoalBuilders;

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

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));

        if (!currentState.CurrentGoals.Any())
        {
            _logger.LogError("Failed to build arithmetic comparison goal: state did not contain any goals.");
            throw new ArgumentException("Must contain at least one term in current goals.", nameof(currentState)); 
        }

        ISimpleTerm goalTerm = currentState.CurrentGoals.First();

        if (goalTerm is not Structure comparisonStruct || comparisonStruct.Children.Count != 2)
        {
            _logger.LogError($"Failed to build arithmetic comparison goal:" +
                $" Goalterm {goalTerm} was not of type struct or did not contain 2 children.");
            throw new ArgumentException("Goal must contain a structure term with two children.", nameof(currentState)); 
        }

        return new ArithmeticComparisonGoal
        (
            _evaluator,
            comparisonStruct.Children.ElementAt(0),
            comparisonStruct.Children.ElementAt(1),
            _predicate,
            currentState.SolutionState,
            _logger
        );
    }
}

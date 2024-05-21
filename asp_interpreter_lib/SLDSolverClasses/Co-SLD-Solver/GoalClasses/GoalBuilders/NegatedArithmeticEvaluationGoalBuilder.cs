using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using asp_interpreter_lib.Unification.Constructive.Disunification;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.GoalBuilders;

internal class NegatedArithmeticEvaluationGoalBuilder : IGoalBuilder
{
    private readonly ArithmeticEvaluator _evaluator;
    private readonly IConstructiveDisunificationAlgorithm _algorithm;
    private readonly ILogger _logger;

    public NegatedArithmeticEvaluationGoalBuilder
    (
        ArithmeticEvaluator evaluator,
        IConstructiveDisunificationAlgorithm algorithm,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        _evaluator = evaluator;
        _algorithm = algorithm;
        _logger = logger;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));

        if (!currentState.CurrentGoals.Any())
        {
            _logger.LogError("Failed to build negated arithmetic evaluation goal: state did not contain any goals.");
            throw new ArgumentException("Must contain at least one goal.", nameof(currentState));
        }

        var goalTerm = currentState.CurrentGoals.First();

        if (goalTerm is not Structure evaluationStruct || evaluationStruct.Children.Count != 2)
        {
            _logger.LogError($"Failed to build negated arithmetic evaluation goal:" +
                    $" Goalterm {goalTerm} was not of type struct or did not contain 2 children.");
            throw new ArgumentException("Next goal must be a structure term with two children.", nameof(currentState));
        }

        return new NegatedArithmeticEvaluationGoal
        (
            _evaluator,
            evaluationStruct.Children.ElementAt(0),
            evaluationStruct.Children.ElementAt(1),
            currentState.SolutionState,
            _algorithm,
            _logger
        );
    }
}

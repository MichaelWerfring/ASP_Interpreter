using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public class ArithmeticEvaluationGoalBuilder : IGoalBuilder
{
    private readonly ArithmeticEvaluator _evaluator;
    private readonly IConstructiveUnificationAlgorithm _algorithm;
    private readonly ILogger _logger;
    private readonly SolverStateUpdater _updater;

    public ArithmeticEvaluationGoalBuilder
    (
        SolverStateUpdater updater,
        ArithmeticEvaluator evaluator,
        IConstructiveUnificationAlgorithm algorithm,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(updater);
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        _evaluator = evaluator;
        _algorithm = algorithm;
        _logger = logger;
        _updater = updater;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));

        if (!currentState.CurrentGoals.Any())
        {
            throw new ArgumentException("Must contain at least one goal.", nameof(currentState)); 
        }

        var goalTerm = currentState.CurrentGoals.First();

        if (goalTerm is not Structure evaluationStruct || evaluationStruct.Children.Count != 2)
        {
            throw new ArgumentException("Next goal must be a structure term with two children.", nameof(currentState)); 
        }

        return new ArithmeticEvaluationGoal
        (
            _updater,
            _evaluator,
            evaluationStruct.Children.ElementAt(0),
            evaluationStruct.Children.ElementAt(1),
            currentState.SolutionState,
            _algorithm,
            _logger
        );
    }
}
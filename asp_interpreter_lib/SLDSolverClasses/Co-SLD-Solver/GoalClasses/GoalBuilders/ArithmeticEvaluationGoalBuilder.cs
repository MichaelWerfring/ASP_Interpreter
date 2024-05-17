using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.Unification.Constructive.Unification;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public class ArithmeticEvaluationGoalBuilder : IGoalBuilder
{
    private readonly ArithmeticEvaluator _evaluator;

    private IConstructiveUnificationAlgorithm _algorithm;

    public ArithmeticEvaluationGoalBuilder(ArithmeticEvaluator evaluator, IConstructiveUnificationAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(algorithm);

        _evaluator = evaluator;
        _algorithm = algorithm;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        if (!currentState.CurrentGoals.Any())
        {
            throw new ArgumentException("Must contain at least one term!", nameof(currentState.CurrentGoals)); 
        }

        var goalTerm = currentState.CurrentGoals.First();

        if (goalTerm is not Structure evaluationStruct || evaluationStruct.Children.Count() != 2)
        {
            throw new ArgumentException("Must contain a structure term with two children.", nameof(currentState.CurrentGoals)); 
        }

        return new ArithmeticEvaluationGoal
        (
            _evaluator,
            evaluationStruct.Children.ElementAt(0),
            evaluationStruct.Children.ElementAt(1),
            currentState.SolutionState,
            _algorithm
        );
    }
}
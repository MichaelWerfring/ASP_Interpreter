using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public class ForallGoalBuilder : IGoalBuilder
{
    private readonly ILogger _logger;
    private readonly GoalSolver _solverForForallGoal;

    public ForallGoalBuilder(ILogger logger, GoalSolver solver)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(solver, nameof(solver));

        _logger = logger;
        _solverForForallGoal = solver;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));

        if (!currentState.CurrentGoals.Any()) 
        {
            _logger.LogError("Failed to forall goal: state did not contain any goals.");
            throw new ArgumentException("Must contain at least one goal.", nameof(currentState)); 
        }

        var goalTerm = currentState.CurrentGoals.First();

        if (goalTerm is not Structure forallStruct || forallStruct.Children.Count != 2)
        {
            _logger.LogError($"Failed to build forall goal:" +
                 $" Goalterm {goalTerm} was not of type struct or did not contain 2 children.");
            throw new ArgumentException("Next goal must be a structure term with two children.", nameof(currentState)); 
        }

        if (forallStruct.Children.ElementAt(0) is not Variable var)
        {
            _logger.LogError($"Failed to build forall goal: first child of {goalTerm} was not a variable.");
            throw new ArgumentException("First child must be a variable.");
        }

        if (forallStruct.Children.ElementAt(1) is not Structure structure)
        {
            _logger.LogError($"Failed to build forall goal: second child of {goalTerm} was not a structure.");
            throw new ArgumentException("First child must be a variable.");
        }

        return new ForallGoal
        (
            _solverForForallGoal,
            var,
            structure,
            currentState.SolutionState,
            _logger
        );
    }
}

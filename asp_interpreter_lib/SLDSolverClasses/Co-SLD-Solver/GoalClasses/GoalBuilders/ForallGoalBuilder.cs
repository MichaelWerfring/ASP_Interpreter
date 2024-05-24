using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public class ForallGoalBuilder : IGoalBuilder
{
    private readonly ILogger _logger;
    private readonly GoalSolver _solverForForallGoal;
    private readonly int _maxSolutionCount;

    public ForallGoalBuilder(ILogger logger, GoalSolver solver, int maxSolutionCount)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(solver, nameof(solver));

        _logger = logger;
        _solverForForallGoal = solver;
        _maxSolutionCount = maxSolutionCount;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));

        if (!currentState.CurrentGoals.Any()) 
        {
            throw new ArgumentException("Must contain at least one goal.", nameof(currentState)); 
        }

        Structure goalTerm = currentState.CurrentGoals.First();

        if (goalTerm.Children.Count != 2)
        {
            throw new ArgumentException("Next goal must be a structure term with two children.", nameof(currentState)); 
        }

        var variableAtPositionOneMaybe = TermFuncs.ReturnVariableOrNone(goalTerm.Children[0]);
        if (!variableAtPositionOneMaybe.HasValue)
        {
            throw new ArgumentException("First child must be a variable.");
        }

        var structureAtPositionTwoMaybe = TermFuncs.ReturnStructureOrNone(goalTerm.Children[1]);
        if (!structureAtPositionTwoMaybe.HasValue)
        {
            throw new ArgumentException("Second child must be a structure.");
        }

        return new ForallGoal
        (
            _solverForForallGoal,
            variableAtPositionOneMaybe.GetValueOrThrow(),
            structureAtPositionTwoMaybe.GetValueOrThrow(),
            currentState.SolutionState,
            _logger,
            _maxSolutionCount
        );
    }
}

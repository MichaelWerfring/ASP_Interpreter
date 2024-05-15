using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class GoalSolver
{
    private readonly SolverStateUpdater _updater = new();

    private readonly CoSLDGoalMapper _goalMapper;

    private readonly IDatabase _database;

    private readonly ILogger _logger;

    public GoalSolver(CoSLDGoalMapper goalMapper, IDatabase database, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(goalMapper, nameof(goalMapper));
        ArgumentNullException.ThrowIfNull(database, nameof(database));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _goalMapper = goalMapper;
        _database = database;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> SolveGoals(CoSldSolverState inputState)
    {      
        if (!inputState.CurrentGoals.Any())
        {
            if (inputState.SolutionState.CHS.Entries.Count != 0) 
                _logger.LogTrace("Solved goal: " + inputState.SolutionState.CHS.Entries.ToList().ListToString());

            yield return new GoalSolution
            (
                inputState.SolutionState.CHS,
                inputState.SolutionState.Mapping,
                inputState.SolutionState.Callstack,
                inputState.SolutionState.NextInternalVariableIndex
            );
            yield break;
        }

        IOption<ICoSLDGoal> goalToSolveMaybe = _goalMapper.GetGoal(inputState, _database);

        if (!goalToSolveMaybe.HasValue)
        {
            yield break;
        }

        ICoSLDGoal goal = goalToSolveMaybe.GetValueOrThrow();
        
        // for each way the goal can be satisified..
        foreach (GoalSolution solution in goal.TrySatisfy())
        {
            CoSldSolverState nextState = _updater.UpdatAfterGoalFulfilled(inputState, solution);
         
            // yield return all the ways the rest of the goals can be satisfied
            foreach (GoalSolution resolution in SolveGoals(nextState))
            {
                yield return resolution;
            }
        }
    }
}

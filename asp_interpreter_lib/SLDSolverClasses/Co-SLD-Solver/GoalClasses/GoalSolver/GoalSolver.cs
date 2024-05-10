using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class GoalSolver
{
    private readonly SolverStateUpdater _updater = new SolverStateUpdater();

    private readonly CoSLDGoalMapper _goalMapper;

    private readonly IDatabase _database;

    public GoalSolver(CoSLDGoalMapper goalMapper, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(goalMapper, nameof(goalMapper));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        _goalMapper = goalMapper;
        _database = database;
    }

    public IEnumerable<GoalSolution> SolveGoals(CoSldSolverState inputState)
    {      
        if (!inputState.CurrentGoals.Any())
        {
            yield return new GoalSolution
            (
                inputState.SolutionState.Set,
                inputState.SolutionState.Mapping,
                inputState.SolutionState.NextInternalVariableIndex
            );
            yield break;
        }

        var stateWithSubstitutedGoals = _updater.GetNewStateWithSubstitutedCurrentGoal(inputState);

        var goalToSolveMaybe = _goalMapper.GetGoal(stateWithSubstitutedGoals, _database);

        if (!goalToSolveMaybe.HasValue)
        {
            yield break;
        }

        var goal = goalToSolveMaybe.GetValueOrThrow();

        // for each way the goal can be satisified..
        foreach(var goalResult in goal.TrySatisfy())
        {
            var newState = _updater.CreateNewStateFromGoalSolution(inputState, goalResult);

            // yield return all the ways the rest of the goals can be satisfied
            foreach (var resolution in SolveGoals(newState))
            {
                yield return resolution; 
            }
        }
    }
}

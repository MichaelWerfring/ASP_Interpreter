using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class GoalSolver
{
    private readonly CoSLDGoalMapper _goalMapper;
    private readonly IDatabase _database;

    public GoalSolver(CoSLDGoalMapper goalMapper, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(goalMapper, nameof(goalMapper));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        _goalMapper = goalMapper;
        _database = database;
    }

    public IEnumerable<GoalSolverResult> SolveGoals(CoSldSolverState currentState)
    {
        if (currentState.CurrentGoals.Count == 0)
        {
            yield return new 
                GoalSolverResult
                (
                    currentState.SolutionState.CurrentSet,
                    currentState.SolutionState.CurrentMapping,
                    currentState.SolutionState.NextInternalVariableIndex
                );
            yield break;
        }

        var GoalToSolveMaybe = _goalMapper.GetGoal(currentState, _database);

        if (!GoalToSolveMaybe.HasValue) { yield break; }

        var results = GoalToSolveMaybe.GetValueOrThrow().TrySatisfy();

        foreach(var result in results)
        {
            foreach(var resolution in SolveGoals(result)) { yield return resolution; }
        }
    }
}

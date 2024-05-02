using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class GoalSolver
{
    private GoalMapper _goalMapper;
    private IDatabase _database;

    public GoalSolver(GoalMapper goalMapper, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(goalMapper, nameof(goalMapper));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        _goalMapper = goalMapper;
        _database = database;
    }

    public IEnumerable<GoalSolverResult> SolveGoals(CoSldSolverState currentState)
    {
        if(currentState.CurrentGoals.Count() == 0)
        {
            yield return new GoalSolverResult(currentState.CurrentSet, currentState.CurrentMapping);
        }

        var GoalToSolveMaybe = _goalMapper.GetGoal(currentState.CurrentGoals.First());

        if (!GoalToSolveMaybe.HasValue) { yield break; }

        var results = GoalToSolveMaybe.GetValueOrThrow().TrySatisfy();

        foreach(var result in results)
        {
            var resolutions = SolveGoals(result);

            foreach(var resolution in resolutions) { yield return resolution; }
        }
    }
}

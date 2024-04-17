using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.GoalMapping;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication;

public class GoalResolver
{
    private GoalMapper _goalmapper;

    public GoalResolver(GoalMapper goalmapper)
    {
        ArgumentNullException.ThrowIfNull(goalmapper);

        _goalmapper = goalmapper;
    }

    public IEnumerable<SolverState> Solve(SolverState state, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(database, nameof(database));
        if(state.CurrentGoals.Count() < 1)
        {
            throw new Exception("Must contain at least one goal.");
        }

        ISimpleTerm goalTerm = state.CurrentGoals.First();

        var goal = _goalmapper.GetGoal(goalTerm);
        if (!goal.HasValue)
        {
            yield break;
        }

        var branches = goal.GetValueOrThrow().TrySatisfy(database, state);

        foreach(var branch in branches)
        {
            yield return branch;
        };
    }
}

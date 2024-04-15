using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.GoalMapping;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals;
using asp_interpreter_lib.Unification.Interfaces;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication;

public class GoalResolver
{
    private GoalMapper _goalmapper;

    public GoalResolver(GoalMapper goalMapper)
    {
        ArgumentNullException.ThrowIfNull(goalMapper, nameof(goalMapper));

        _goalmapper = goalMapper;
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

        var branches = goal.TrySatisfy(database, state);
        
        foreach (var resolution in branches)
        {
            yield return resolution;
        }
    }
}

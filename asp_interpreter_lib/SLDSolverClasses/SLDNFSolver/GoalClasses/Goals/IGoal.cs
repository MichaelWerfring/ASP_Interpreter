using asp_interpreter_lib.InternalProgramClasses.Database;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalClasses.Goals;

public interface IGoal
{
    public IEnumerable<SolverState> TrySatisfy(IDatabase database, SolverState state);
}

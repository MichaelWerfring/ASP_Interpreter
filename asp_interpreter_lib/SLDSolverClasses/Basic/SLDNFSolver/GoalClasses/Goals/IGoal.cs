using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals;

public interface IGoal
{
    public IEnumerable<SolverState> TrySatisfy(IDatabase database, SolverState state);
}

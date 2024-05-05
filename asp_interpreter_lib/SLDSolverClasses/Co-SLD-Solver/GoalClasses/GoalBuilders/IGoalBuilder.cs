using asp_interpreter_lib.InternalProgramClasses.Database;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public interface IGoalBuilder
{
    public ICoSLDGoal BuildGoal(CoSldSolverState currentState, IDatabase database);
}

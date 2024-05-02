using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals
{
    public class DatabaseUnificationGoalBuilder : IGoalBuilder
    {
        public ICoSLDGoal BuildGoal(CoSldSolverState currentState, IDatabase database)
        {
            throw new NotImplementedException();
        }
    }
}
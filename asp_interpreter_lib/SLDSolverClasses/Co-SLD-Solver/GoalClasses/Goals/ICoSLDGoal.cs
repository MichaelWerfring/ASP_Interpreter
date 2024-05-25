namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public interface ICoSLDGoal
{
    public IEnumerable<GoalSolution> TrySatisfy();
}

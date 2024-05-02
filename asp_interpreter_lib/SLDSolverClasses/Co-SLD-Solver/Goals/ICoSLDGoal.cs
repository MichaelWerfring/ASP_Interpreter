namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public interface ICoSLDGoal
{
    public IEnumerable<CoSldSolverState> TrySatisfy();

}

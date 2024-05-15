using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CoSldSolverState
{
    public CoSldSolverState(IEnumerable<ISimpleTerm> currentGoals, SolutionState solutionState)
    {
        ArgumentNullException.ThrowIfNull(currentGoals, nameof(currentGoals));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));

        SolutionState = solutionState;
        CurrentGoals = currentGoals.ToList();      
    }

    public IEnumerable<ISimpleTerm> CurrentGoals { get; }

    public SolutionState SolutionState { get; }
}

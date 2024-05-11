using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.Util;
using System.Text;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CoSldSolverState
{
    public CoSldSolverState(IEnumerable<ISimpleTerm> currentGoals, SolutionState solutionState)
    {
        ArgumentNullException.ThrowIfNull(currentGoals, nameof(currentGoals));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));

        SolutionState = solutionState;
        CurrentGoals = currentGoals;      
    }

    public IEnumerable<ISimpleTerm> CurrentGoals { get; }

    public SolutionState SolutionState { get; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Solver state:");
        sb.AppendLine("Goals:");
        sb.AppendLine(CurrentGoals.ToList().ListToString());
        sb.AppendLine("Solution state:");
        sb.AppendLine(SolutionState.ToString());

        return sb.ToString();
    }
}

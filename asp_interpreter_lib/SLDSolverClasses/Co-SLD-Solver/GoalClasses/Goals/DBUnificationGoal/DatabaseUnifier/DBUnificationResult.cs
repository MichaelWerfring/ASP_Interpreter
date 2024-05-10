using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;

public class DBUnificationResult
{
    public DBUnificationResult(IEnumerable<ISimpleTerm> subgoals, VariableMapping mapping, int nextInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(subgoals, nameof(subgoals));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        Subgoals = subgoals;
        VariableMapping = mapping;
        NextInternalIndex = nextInternalIndex;
    }

    public IEnumerable<ISimpleTerm> Subgoals { get; }

    public VariableMapping VariableMapping { get; }

    public int NextInternalIndex { get; }
}

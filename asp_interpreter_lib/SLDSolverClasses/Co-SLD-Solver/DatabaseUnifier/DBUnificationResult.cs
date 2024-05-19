using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;

public class DBUnificationResult
{
    public DBUnificationResult(IEnumerable<ISimpleTerm> renamedClause, VariableMapping unifyingMapping, int nextInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(renamedClause, nameof(renamedClause));
        ArgumentNullException.ThrowIfNull(unifyingMapping, nameof(unifyingMapping));

        RenamedClause = renamedClause;
        UnifyingMapping = unifyingMapping;
        NextInternalIndex = nextInternalIndex;
    }

    public IEnumerable<ISimpleTerm> RenamedClause { get; }

    public VariableMapping UnifyingMapping { get; }

    public int NextInternalIndex { get; }
}

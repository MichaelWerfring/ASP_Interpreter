using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class GoalSolverResult
{
    public GoalSolverResult(CoinductiveHypothesisSet resultSet, VariableMapping resultMapping, int nextInternalVariableIndex)
    {
        ArgumentNullException.ThrowIfNull(resultSet, nameof(resultSet));
        ArgumentNullException.ThrowIfNull(resultMapping, nameof(resultMapping));

        ResultSet = resultSet;
        ResultMapping = resultMapping;
        NextInternalVariable = nextInternalVariableIndex;
    }

    public CoinductiveHypothesisSet ResultSet { get; }

    public VariableMapping ResultMapping { get; }

    public int NextInternalVariable { get; }
}

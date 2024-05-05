using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;

public class CoSLDSolution
{
    public CoSLDSolution(CoinductiveHypothesisSet set, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(set, nameof(set));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        SolutionSet = set;
        SolutionMapping = mapping;
    }

    public CoinductiveHypothesisSet SolutionSet { get; }

    public VariableMapping SolutionMapping { get; }
}

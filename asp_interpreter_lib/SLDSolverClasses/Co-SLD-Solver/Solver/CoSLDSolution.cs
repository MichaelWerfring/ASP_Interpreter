using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;

public class CoSLDSolution
{
    public CoSLDSolution(IEnumerable<ISimpleTerm> chsEntries, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(chsEntries, nameof(chsEntries));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        CHSEntries = chsEntries;
        SolutionMapping = mapping;
    }

    public IEnumerable<ISimpleTerm> CHSEntries { get; }

    public VariableMapping SolutionMapping { get; }
}

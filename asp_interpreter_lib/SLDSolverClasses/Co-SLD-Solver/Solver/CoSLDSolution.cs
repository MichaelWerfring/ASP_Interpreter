using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;

public class CoSLDSolution
{
    public CoSLDSolution(IEnumerable<ISimpleTerm> set, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(set, nameof(set));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        CHSEntries = set;
        SolutionMapping = mapping;
    }

    public IEnumerable<ISimpleTerm> CHSEntries { get; }

    public VariableMapping SolutionMapping { get; }
}

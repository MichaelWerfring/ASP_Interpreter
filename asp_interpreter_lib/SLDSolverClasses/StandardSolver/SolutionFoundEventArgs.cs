using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;

namespace asp_interpreter_lib.SLDSolverClasses.StandardSolver;

public class SolutionFoundEventArgs
{
    public SolutionFoundEventArgs(Dictionary<Variable, ISimpleTerm> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);
        Mapping = mapping;
    }

    public Dictionary<Variable, ISimpleTerm> Mapping { get; }
}
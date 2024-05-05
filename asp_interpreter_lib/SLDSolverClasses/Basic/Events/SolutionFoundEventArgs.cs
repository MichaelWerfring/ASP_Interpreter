using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.Events;

public class SolutionFoundEventArgs
{
    public SolutionFoundEventArgs(Dictionary<Variable, ISimpleTerm> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);
        Mapping = mapping;
    }

    public Dictionary<Variable, ISimpleTerm> Mapping { get; }
}
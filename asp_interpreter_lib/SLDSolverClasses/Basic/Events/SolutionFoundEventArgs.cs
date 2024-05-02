using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

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
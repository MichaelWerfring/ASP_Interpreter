using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.SLDSolverClasses.Events;

public class SolutionFoundEventArgs
{
    public SolutionFoundEventArgs(Dictionary<Variable, ISimpleTerm> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);
        Mapping = mapping;
    }

    public Dictionary<Variable, ISimpleTerm> Mapping { get; }
}
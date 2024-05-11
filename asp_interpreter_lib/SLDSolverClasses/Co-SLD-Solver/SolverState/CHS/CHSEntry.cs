using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState.CHS;

public class CHSEntry
{
    public CHSEntry(ISimpleTerm term, bool hasSucceeded)
    {
        ArgumentNullException.ThrowIfNull(term);

        Term = term;
        HasSucceded = hasSucceeded;
    }

    public ISimpleTerm Term { get; }

    public bool HasSucceded { get; }

    public override string ToString()
    {
        return $"{Term}:{HasSucceded}";
    }
}

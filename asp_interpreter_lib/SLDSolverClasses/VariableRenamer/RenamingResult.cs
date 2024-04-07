using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;

namespace asp_interpreter_lib.SLDSolverClasses.VariableRenamer;

public class RenamingResult
{
    public RenamingResult(IEnumerable<IInternalTerm> clause, int nextInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        RenamedClause = clause;
        NextInternalIndex = nextInternalIndex;
    }

    public IEnumerable<IInternalTerm> RenamedClause { get; }

    public int NextInternalIndex { get; }
}

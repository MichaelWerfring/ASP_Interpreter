using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.SLDSolverClasses.VariableRenaming;

public class RenamingResult
{
    public RenamingResult(IEnumerable<ISimpleTerm> clause, int nextInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        RenamedClause = clause;
        NextInternalIndex = nextInternalIndex;
    }

    public IEnumerable<ISimpleTerm> RenamedClause { get; }

    public int NextInternalIndex { get; }
}

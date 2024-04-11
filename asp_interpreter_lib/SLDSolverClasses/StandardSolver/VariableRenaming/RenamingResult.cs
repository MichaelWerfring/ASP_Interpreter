using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;

namespace asp_interpreter_lib.SLDSolverClasses.StandardSolver.VariableRenamer;

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

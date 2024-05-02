using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

namespace asp_interpreter_lib.InternalProgramClasses.Database;

public class StandardDatabase : IDatabase
{
    private IEnumerable<IEnumerable<ISimpleTerm>> _clauses;

    public StandardDatabase(IEnumerable<IEnumerable<ISimpleTerm>> clauses)
    {
        ArgumentNullException.ThrowIfNull(clauses);
        if (clauses.Any((clause) => clause == null || clause.Count() < 1))
        {
            throw new ArgumentException("Must not contain null clauses, or clauses with not at least one term");
        }

        _clauses = clauses;
    }

    public IEnumerable<IEnumerable<ISimpleTerm>> GetMatchingClauses(ISimpleTerm term)
    {
        return _clauses;
    }
}

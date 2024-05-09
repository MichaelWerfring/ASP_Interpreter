using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

namespace asp_interpreter_lib.InternalProgramClasses.Database;

public class BasicDatabase : IDatabase
{
    private IEnumerable<IEnumerable<Structure>> _clauses;

    public BasicDatabase(IEnumerable<IEnumerable<Structure>> clauses)
    {
        ArgumentNullException.ThrowIfNull(clauses);
        if (clauses.Any((clause) => clause == null || clause.Count() < 1))
        {
            throw new ArgumentException("Must not contain null clauses, or clauses with not at least one term");
        }

        _clauses = clauses;
    }

    public IEnumerable<IEnumerable<Structure>> GetPotentialUnifications(Structure term)
    {
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        foreach (var clause in _clauses)
        {
            yield return clause;
        }
    }
}

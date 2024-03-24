namespace asp_interpreter_lib.Types;

public class AspProgram
{
    private List<Statement> _statements;
    private Query _query;

    public AspProgram(List<Statement> statements, Query query)
    {
        Statements = statements;
        Query = query;
    }

    public List<Statement> Statements
    {
        get => _statements;
        private set => _statements = value ?? throw new ArgumentNullException(nameof(Statements));
    }

    public Query Query
    {
        get => _query;
        private set => _query = value ?? throw new ArgumentNullException(nameof(Query));
    }
}
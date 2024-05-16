using System.Text;

using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types;

public class AspProgram : IVisitableType
{
    private List<Statement> _statements;
    private IOption<Query> _query;
    private Dictionary<string, Explanation> _explainations;

    public AspProgram(List<Statement> statements, IOption<Query> query, Dictionary<string, Explanation> explanations)
    {
        Statements = statements;
        Query = query;
        Explanations = explanations;
    }

    public List<Statement> Statements
    {
        get => _statements;
        private set => _statements = value ?? throw new ArgumentNullException(nameof(Statements));
    }

    public IOption<Query> Query
    {
        get => _query;
        private set => _query = value ?? throw new ArgumentNullException(nameof(Query));
    }

    public Dictionary<string, Explanation> Explanations 
    {
        get => _explainations;
        private set => _explainations = value ?? throw new ArgumentNullException(nameof(Explanations));
    }


    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var statement in Statements)
        {
            builder.Append(statement.ToString());
            builder.AppendLine();
        }

        if (Query.HasValue) { builder.Append(Query.GetValueOrThrow().ToString()); };
        builder.AppendLine();

        return builder.ToString();
    }

    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}
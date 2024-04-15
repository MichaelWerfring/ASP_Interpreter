using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.ListExtensions;
using System.Text;

namespace asp_interpreter_lib.InternalProgramClasses.InternalProgram;

public class InternalAspProgram 
{
    public InternalAspProgram(IEnumerable<IEnumerable<ISimpleTerm>> statements, IEnumerable<ISimpleTerm> query)
    {
        ArgumentNullException.ThrowIfNull(statements);
        ArgumentNullException.ThrowIfNull(query);

        if
        (
            statements.Any
            (
                (statement) => statement == null
                ||
                statement.Count() < 1
                ||
                statement.Any((term) => term == null)
            )
        )
        {
            throw new ArgumentException
            (
                "Must: " +
                "not contain nulls, " +
                "all statements must have at least one term " +
                "and no nulls", nameof(statements)
            );
        }

        if (query.Any((term) => term == null )) 
        {
            throw new ArgumentException("Must not contain any nulls.", nameof(query));
        }

        Statements = statements;
        Query = query;
    }

    public IEnumerable<IEnumerable<ISimpleTerm>> Statements { get; }

    public IEnumerable<ISimpleTerm> Query { get; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var statement in Statements)
        {
            builder.Append(statement.ToList().ListToString());
            builder.AppendLine();
        }

        builder.Append(Query.ToList().ListToString());
        builder.AppendLine();

        return builder.ToString();
    }
}
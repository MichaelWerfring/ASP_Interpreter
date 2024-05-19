using System.Text;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Util;

namespace asp_interpreter_lib.InternalProgramClasses.InternalProgram;

public class InternalAspProgram 
{
    public InternalAspProgram(IEnumerable<IEnumerable<Structure>> statements, IEnumerable<Structure> query)
    {
        ArgumentNullException.ThrowIfNull(statements);
        ArgumentNullException.ThrowIfNull(query);

        if
        (
            statements.Any
            (
                (statement) => statement == null
                ||
                !statement.Any()
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

    public IEnumerable<IEnumerable<Structure>> Statements { get; }

    public IEnumerable<Structure> Query { get; }

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
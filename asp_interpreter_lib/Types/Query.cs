using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types;

public class Query
{
    private Literal _literal;

    public Query(Literal literal)
    {
        if (literal.HasNafNegation)
        {
            throw new ArgumentException("Query cannot have NAF negation.");
        }
        
        Literal = literal;
    }

    public Literal Literal
    {
        get => _literal;
        private set => _literal = value ?? throw new ArgumentNullException(nameof(Literal));
    }

    public override string ToString()
    {
        return $"?- {Literal.ToString()}.";
    }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}
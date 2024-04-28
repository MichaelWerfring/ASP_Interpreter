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
        
        ClassicalLiteral = literal;
    }

    public Literal ClassicalLiteral
    {
        get => _literal;
        private set => _literal = value ?? throw new ArgumentNullException(nameof(ClassicalLiteral));
    }

    public override string ToString()
    {
        return $"?- {ClassicalLiteral.ToString()}.";
    }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}
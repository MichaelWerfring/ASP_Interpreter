using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types;

public class Query
{
    private ClassicalLiteral _classicalLiteral;

    public Query(ClassicalLiteral classicalLiteral)
    {
        ClassicalLiteral = classicalLiteral;
    }

    public ClassicalLiteral ClassicalLiteral
    {
        get => _classicalLiteral;
        private set => _classicalLiteral = value ?? throw new ArgumentNullException(nameof(ClassicalLiteral));
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
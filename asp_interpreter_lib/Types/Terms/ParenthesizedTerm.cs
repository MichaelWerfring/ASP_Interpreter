using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.Terms;

public class ParenthesizedTerm: ITerm
{
    private ITerm _term;

    public ParenthesizedTerm(ITerm term)
    {
        Term = term;
    }

    public ITerm Term
    {
        get => _term;
        private set => _term = value ?? throw new ArgumentNullException(nameof(Term), "Term cannot be null!");
    }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"({Term.ToString()})";
    }
}
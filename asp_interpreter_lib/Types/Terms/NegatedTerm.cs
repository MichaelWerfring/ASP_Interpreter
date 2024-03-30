using asp_interpreter_lib.Types.TypeVisitors;
using System.Text;
using asp_interpreter_lib.ErrorHandling;

namespace asp_interpreter_lib.Types.Terms;

public class NegatedTerm: ITerm
{
    private ITerm _term;

    public NegatedTerm(ITerm term)
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
        return $"NegatedTerm({Term.ToString()})";
    }
}
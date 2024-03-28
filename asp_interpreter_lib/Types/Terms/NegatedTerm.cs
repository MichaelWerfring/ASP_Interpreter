using asp_interpreter_lib.Types.TypeVisitors;
using System.Text;

namespace asp_interpreter_lib.Types.Terms;

public class NegatedTerm: Term
{
    private Term _term;

    public NegatedTerm(Term term)
    {
        Term = term;
    }

    public Term Term
    {
        get => _term;
        private set => _term = value ?? throw new ArgumentNullException(nameof(Term), "Term cannot be null!");
    }
    
    public override T Accept<T>(ITermVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
    
    public override void Accept(ITermVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"NegatedTerm({Term.ToString()})";
    }
}
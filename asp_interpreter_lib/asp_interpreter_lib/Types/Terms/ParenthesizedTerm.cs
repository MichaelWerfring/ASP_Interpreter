using asp_interpreter_lib.Types.Terms.TermConversion;

namespace asp_interpreter_lib.Types.Terms;

public class ParenthesizedTerm: Term
{
    private Term _term;

    public ParenthesizedTerm(Term term)
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
        return visitor.Visit(this);
    }
}
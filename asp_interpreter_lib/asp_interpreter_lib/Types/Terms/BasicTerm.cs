using asp_interpreter_lib.Types.Terms.TermConversion;

namespace asp_interpreter_lib.Types.Terms;

public class BasicTerm: Term
{
    private string _identifier;
    private List<Term> _terms;

    public BasicTerm(string identifier, List<Term> terms)
    {
        Identifier = identifier;
        Terms = terms;
    }

    public string Identifier
    {
        get => _identifier;
        private set
        {
            if (string.IsNullOrWhiteSpace(value) || value == string.Empty )
            {
                throw new ArgumentException("The given Identifier must not be null, whitespace or empty!",
                    nameof(Identifier));
            }

            _identifier = value;
        }
    }

    public List<Term> Terms
    {
        get => _terms;
        set => _terms = value ?? throw new ArgumentNullException(nameof(Terms));
    }
    
    public override T Accept<T>(ITermVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
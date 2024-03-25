using asp_interpreter_lib.Types.TypeVisitors;
using System.Text;

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
        var builder = new StringBuilder();
        builder.Append(Identifier);
        builder.Append('(');
        for(int i = 0; i < Terms.Count - 1; i++)
        {
            builder.Append($"{Terms[i].ToString()}, ");
        }
        builder.Append($"{Terms[Terms.Count - 1].ToString()}");
        builder.Append(')');

        return builder.ToString();
    }
}
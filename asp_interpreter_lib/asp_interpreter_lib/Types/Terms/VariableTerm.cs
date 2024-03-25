using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.Terms;

public class VariableTerm: Term
{
    private string _identifier;

    public VariableTerm(string identifier)
    {
        Identifier = identifier;
    }

    public string Identifier
    {
        get => _identifier;
        private set
        {
            if (string.IsNullOrWhiteSpace(value) || value == string.Empty)
            {
                throw new ArgumentException("Identifier cannot be null, empty or whitespace!", 
                    nameof(Identifier));
            }
            
            _identifier = value;
        }
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
        return Identifier;
    }
}
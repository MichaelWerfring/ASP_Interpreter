using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.Terms;

public class VariableTerm: ITerm
{
    private string _identifier;

    public VariableTerm(string identifier)
    {
        Identifier = identifier;
    }

    public string Identifier
    {
        get => _identifier;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value == string.Empty)
            {
                throw new ArgumentException("Identifier cannot be null, empty or whitespace!", 
                    nameof(Identifier));
            }
            
            _identifier = value;
        }
    }

    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return Identifier;
    }
}
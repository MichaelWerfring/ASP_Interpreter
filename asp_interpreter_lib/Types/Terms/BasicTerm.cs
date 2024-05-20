using asp_interpreter_lib.Types.TypeVisitors;
using System.Text;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.Terms;

public class BasicTerm: ITerm
{
    private string _identifier;
    private List<ITerm> _terms;

    public BasicTerm(string identifier, List<ITerm> terms)
    {
        Identifier = identifier;
        Terms = terms;
    }

    public string Identifier
    {
        get => _identifier;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value == string.Empty )
            {
                throw new ArgumentException("The given Identifier must not be null, whitespace or empty!",
                    nameof(Identifier));
            }

            _identifier = value;
        }
    }

    public List<ITerm> Terms
    {
        get => _terms;
        set => _terms = value ?? throw new ArgumentNullException(nameof(Terms));
    }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(Identifier);

        if(Terms.Count > 0)
        {
            builder.Append('(');
            builder.Append(Terms.ListToString());
            builder.Append(')');
        }

        return builder.ToString();
    }
}
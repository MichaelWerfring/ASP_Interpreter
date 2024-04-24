using System.Text;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.ListExtensions;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types;

public class Literal : Goal
{
    private List<ITerm> _terms;
    private string _identifier;

    public Literal(string identifier, bool hasNafNegation, bool hasStrongNegation, List<ITerm> terms) 
    {
        Identifier = identifier;
        Terms = terms;
        HasStrongNegation = hasStrongNegation;
        HasNafNegation = hasNafNegation;
    }

    public Literal()
    {
    }

    public List<ITerm> Terms
    {
        get => _terms;
        set => _terms = value ?? throw new ArgumentNullException(nameof(Terms));
    }

    //Negated in this context means classical negation 
    public bool HasStrongNegation { get; set; }
    
    public bool HasNafNegation { get; set; }

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

    public override string ToString()
    {
        var builder = new StringBuilder();

        if(HasNafNegation)
        {
            builder.Append("not ");
        }
        
        if(HasStrongNegation)
        {
            builder.Append('-');
        }

        builder.Append(Identifier);

        if (Terms.Count > 0)
        {
            builder.Append('(');
            builder.Append(Terms.ListToString());
            builder.Append(')');
        }

        return builder.ToString();
    }
    
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}
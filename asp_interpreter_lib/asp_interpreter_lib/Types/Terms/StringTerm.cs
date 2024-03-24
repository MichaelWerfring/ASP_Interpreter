using asp_interpreter_lib.Types.Terms.TermConversion;

namespace asp_interpreter_lib.Types.Terms;

public class StringTerm: Term
{
    private string _value;

    public StringTerm(string value)
    {
        Value = value;
    }

    //Allow empty strings and whitespaces 
    public string Value
    {
        get => _value;
        private set => _value = value ?? throw new ArgumentNullException(nameof(Value),"Value cannot be null!");
    }
    
    public override T Accept<T>(ITermVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
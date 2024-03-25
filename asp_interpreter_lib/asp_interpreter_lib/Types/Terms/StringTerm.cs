using asp_interpreter_lib.Types.TypeVisitors;

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
        return $"StringTerm({Value})";
    }
}
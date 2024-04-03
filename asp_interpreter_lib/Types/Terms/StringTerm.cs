using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.Terms;

public class StringTerm: ITerm
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
    
    public override string ToString()
    {
        return Value;
    }

    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}
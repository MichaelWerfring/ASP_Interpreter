using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.Terms;

public class NumberTerm: ITerm
{
    public NumberTerm(int value)
    {
        Value = value;
    }
    
    //according to the grammar, the value
    //of a number term allows only integer
    public int Value { get; private set; }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
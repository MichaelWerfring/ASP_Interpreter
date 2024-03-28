using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.Terms;

public class AnonymusVariableTerm : Term
{
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
        return "_";
    }
}
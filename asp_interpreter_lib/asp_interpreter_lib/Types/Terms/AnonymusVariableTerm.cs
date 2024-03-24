using asp_interpreter_lib.Types.Terms.TermConversion;

namespace asp_interpreter_lib.Types.Terms;

public class AnonymusVariableTerm : Term
{
    public override T Accept<T>(ITermVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
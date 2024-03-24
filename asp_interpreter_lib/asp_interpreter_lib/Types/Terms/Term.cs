using asp_interpreter_lib.Types.Terms.TermConversion;

namespace asp_interpreter_lib.Types.Terms;

public abstract class Term
{
    public abstract T Accept<T>(ITermVisitor<T> visitor);
}
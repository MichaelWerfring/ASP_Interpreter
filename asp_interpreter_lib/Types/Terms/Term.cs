using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.Terms;

public abstract class Term
{
    public abstract T Accept<T>(ITermVisitor<T> visitor);
    public abstract void Accept(ITermVisitor visitor);

    public abstract override string ToString();
}
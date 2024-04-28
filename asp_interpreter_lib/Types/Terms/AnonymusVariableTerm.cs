using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.Terms;

public class AnonymusVariableTerm : ITerm
{
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return "_";
    }
}
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.Terms;

public abstract class ListTerm : ITerm
{
    public abstract override string ToString();
    
    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
}
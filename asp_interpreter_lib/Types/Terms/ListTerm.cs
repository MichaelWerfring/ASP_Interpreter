using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.Terms;

public abstract class ListTerm : ITerm
{
    public abstract override string ToString();
    
    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
}
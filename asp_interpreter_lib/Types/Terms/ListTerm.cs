using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types.Terms;

public abstract class ListTerm : ITerm
{
    public abstract override string ToString();
    
    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
}
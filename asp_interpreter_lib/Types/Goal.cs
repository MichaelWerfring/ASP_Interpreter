using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types;

public abstract class Goal : IVisitableType
{
    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
    
    public abstract override string ToString();
}
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types;

public abstract class Goal : IVisitableType
{
    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
    
    public abstract override string ToString();
}
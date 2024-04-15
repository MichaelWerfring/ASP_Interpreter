using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.BinaryOperations;

public abstract class BinaryOperator : IVisitableType
{
    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
}
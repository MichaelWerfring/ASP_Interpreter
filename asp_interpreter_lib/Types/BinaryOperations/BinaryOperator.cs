using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.BinaryOperations;

public abstract class BinaryOperator : IVisitableType
{
    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
}
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types.BinaryOperations;

public abstract class BinaryOperator : IVisitableType
{
    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
}
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.ArithmeticOperations;

public abstract class ArithmeticOperation
{
    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);

    public abstract override string ToString();
}
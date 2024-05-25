using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types.ArithmeticOperations;

public abstract class ArithmeticOperation
{
    public abstract int Evaluate(int l, int r);

    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);

    public abstract override string ToString();
}
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.ArithmeticOperations;

public class Minus : ArithmeticOperation
{
    public override int Evaluate(int l, int r)
    {
        return l - r;
    }

    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"-";
    }
}
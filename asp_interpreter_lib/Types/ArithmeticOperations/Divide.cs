using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types.ArithmeticOperations;

public class Divide : ArithmeticOperation
{
    public override int Evaluate(int l, int r)
    {
        if(r == 0)
        {
            throw new DivideByZeroException();
        }

        return l /r ;
    }

    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"/";
    }
}
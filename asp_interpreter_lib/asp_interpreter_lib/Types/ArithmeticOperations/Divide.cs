using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.ArithmeticOperations;

public class Divide(Term left, Term right) : ArithmeticOperation(left, right)
{
    public override Term Evaluate()
    {
        var visitor = new TermToNumberConverter();
        var leftValue = Left.Accept(visitor);
        var rightValue = Right.Accept(visitor);

        if (rightValue == 0)
        {
            throw new DivideByZeroException("The right term of a division cannot be zero.");
        }
        
        return new NumberTerm(leftValue / rightValue);
    }

    public override string ToString()
    {
        return $"{Left.ToString()} / {Right.ToString()}";
    }
}
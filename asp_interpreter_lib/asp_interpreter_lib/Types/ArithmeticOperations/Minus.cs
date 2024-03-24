using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.ArithmeticOperations;

public class Minus(Term left, Term right) : ArithmeticOperation(left, right)
{
    public override Term Evaluate()
    {
        var visitor = new TermToNumberConverter();
        var leftValue = Left.Accept(visitor);
        var rightValue = Right.Accept(visitor);
        return new NumberTerm(leftValue - rightValue);
    }
}
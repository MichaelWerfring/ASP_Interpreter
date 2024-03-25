using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.ArithmeticOperations;

public class Multiply(Term left, Term right) : ArithmeticOperation(left, right)
{
    public override Term Evaluate()
    {
        var visitor = new TermToNumberConverter();
        var leftValue = Left.Accept(visitor);
        var rightValue = Right.Accept(visitor);
        return new NumberTerm(leftValue * rightValue);
    }
}
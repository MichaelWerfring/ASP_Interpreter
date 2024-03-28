using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.ArithmeticOperations;

public class Multiply(Term left, Term right) : ArithmeticOperation(left, right)
{
    public override Term Evaluate()
    {
        var visitor = new TermToNumberConverter();
        var left = Left.Accept(visitor);
        var right = Right.Accept(visitor);

        left.IfHasNoValue(()=> throw new InvalidOperationException(
            "The left term of a division must be a number."));
        right.IfHasNoValue(()=> throw new InvalidOperationException(
            "The right term of a division must be a number."));
        
        int rightValue = right.GetValueOrThrow();
        int leftValue = left.GetValueOrThrow();
        
        return new NumberTerm(leftValue * rightValue);
    }

    public override string ToString()
    {
        return $"{Left.ToString()} * {Right.ToString()}";
    }
}
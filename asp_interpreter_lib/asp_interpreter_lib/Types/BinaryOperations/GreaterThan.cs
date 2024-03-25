using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.BinaryOperations;

public class GreaterThan(Term left, Term right) : BinaryOperation(left, right)
{
    public override bool Evaluate()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"{Left.ToString()} > {Right.ToString()}";
    }
}
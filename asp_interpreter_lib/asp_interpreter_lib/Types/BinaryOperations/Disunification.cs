using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.BinaryOperations;

public class Disunification : BinaryOperator 
{
    public override bool Evaluate(Term left, Term right)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"{Left.ToString()} \\= {Right.ToString()}";
    }
}
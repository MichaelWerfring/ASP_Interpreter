using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.BinaryOperations;

public class Equality: BinaryOperator 
{
    public override bool Evaluate(ITerm left, ITerm right)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "=";
    }
}
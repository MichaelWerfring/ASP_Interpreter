namespace asp_interpreter_lib.Types.ArithmeticOperations;

public class Plus: ArithmeticOperation
{
    public override int Evaluate(int left, int right)
    {
        return left + right;
    }
}
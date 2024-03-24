namespace asp_interpreter_lib.Types.ArithmeticOperations;

public class Divide(int left, int right) : ArithmeticOperation(left, right)
{
    public override int Evaluate()
    {
        return Left / Right;
    }
}
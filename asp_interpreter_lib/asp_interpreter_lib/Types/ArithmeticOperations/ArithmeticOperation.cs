namespace asp_interpreter_lib.Types.ArithmeticOperations;

public abstract class ArithmeticOperation
{
    protected ArithmeticOperation(int left ,int right)
    {
        Left = left;
        Right = right;
    }
    
    public abstract int Evaluate();
    
    public int Left { get; private set; }
    
    public int Right { get; private set; }
}
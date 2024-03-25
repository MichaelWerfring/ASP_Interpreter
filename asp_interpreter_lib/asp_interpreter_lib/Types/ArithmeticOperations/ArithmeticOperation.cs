using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.ArithmeticOperations;

public abstract class ArithmeticOperation
{
    private Term _left;
    private Term _right;

    protected ArithmeticOperation(Term left ,Term right)
    {
        Left = left;
        Right = right;
    }
    
    public abstract Term Evaluate();

    public Term Left
    {
        get => _left;
        private set => _left = value ?? throw new ArgumentNullException(nameof(Left));
    }

    public Term Right
    {
        get => _right;
        private set => _right = value ?? throw new ArgumentNullException(nameof(Right));
    }

    public abstract override string ToString();
}
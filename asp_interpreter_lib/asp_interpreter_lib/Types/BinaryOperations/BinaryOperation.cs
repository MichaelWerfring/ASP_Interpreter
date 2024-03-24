using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.BinaryOperations;

public abstract class BinaryOperation
{
    private Term _left;
    private Term _right;

    protected BinaryOperation(Term left, Term right)
    {
        Left = left;
        Right = right;
    }

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

    public abstract bool Evaluate();
}
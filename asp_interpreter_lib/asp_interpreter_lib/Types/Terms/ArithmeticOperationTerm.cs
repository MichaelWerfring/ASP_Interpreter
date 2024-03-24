using asp_interpreter_lib.Types.ArithmeticOperations;

namespace asp_interpreter_lib.Types.Terms;

public class ArithmeticOperationTerm : Term
{
    private ArithmeticOperation _operation;
    private Term _left;
    private Term _right;

    public ArithmeticOperationTerm(Term left, ArithmeticOperation operation, Term right)
    {
        Left = left;
        Operation = operation;
        Right = right;
    }

    public ArithmeticOperation Operation
    {
        get => _operation;
        private set => _operation = value ?? throw new ArgumentNullException(nameof(Operation));
    }

    public Term Left
    {
        get => _left;
        private set => _left = value?? throw new ArgumentNullException(nameof(Left));
    }

    public Term Right
    {
        get => _right;
        private set => _right = value ?? throw new ArgumentNullException(nameof(Right));
    }
}
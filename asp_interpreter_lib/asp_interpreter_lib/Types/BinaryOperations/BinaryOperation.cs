using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.BinaryOperations;

public class BinaryOperation
{
    private Term _left;
    private Term _right;
    private BinaryOperator _binaryOperator;

    public BinaryOperation(Term left, BinaryOperator binaryOperator, Term right)
    {
        Left = left;
        BinaryOperator = binaryOperator;
        Right = right;
    }

    public Term Left
    {
        get => _left;
        private set => _left = value ?? throw new ArgumentNullException(nameof(Left));
    }

    public BinaryOperator BinaryOperator
    {
        get => _binaryOperator;
        private set => _binaryOperator = value ?? throw new ArgumentNullException(nameof(BinaryOperator));
    }

    public Term Right
    {
        get => _right;
        private set => _right = value ?? throw new ArgumentNullException(nameof(Right));
    }
    
    //Evaluate by delegating to the operator
}
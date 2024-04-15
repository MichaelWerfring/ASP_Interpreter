using asp_interpreter_lib.Types.Terms;
using System.Data;

namespace asp_interpreter_lib.Types.BinaryOperations;

public class BinaryOperation
{
    private ITerm _left;
    private ITerm _right;
    private BinaryOperator _binaryOperator;

    public BinaryOperation(ITerm left, BinaryOperator binaryOperator,ITerm right)
    {
        Left = left;
        Right = right;
        BinaryOperator = binaryOperator;
    }

    public ITerm Left
    {
        get => _left;
        private set => _left = value ?? throw new ArgumentNullException(nameof(Left));
    }

    public ITerm Right
    {
        get => _right;
        private set => _right = value ?? throw new ArgumentNullException(nameof(Right));
    }

    public BinaryOperator BinaryOperator
    {
        get => _binaryOperator;
        set => _binaryOperator = value ?? throw new ArgumentNullException(nameof(BinaryOperator));
    }
    
    public override string ToString()
    {
        return $"{Left.ToString()} {BinaryOperator.ToString()} {Right.ToString()}";
    }
}
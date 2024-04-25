using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types;

public class BinaryOperation : Goal
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

    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}
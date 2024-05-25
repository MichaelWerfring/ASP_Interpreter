﻿using Asp_interpreter_lib.Types.ArithmeticOperations;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types.Terms;

public class ArithmeticOperationTerm : ITerm
{
    private ArithmeticOperation _operation;
    private ITerm _left;
    private ITerm _right;

    public ArithmeticOperationTerm(ITerm left,ArithmeticOperation operation, ITerm right)
    {
        Left = left;
        Operation = operation;
        Right = right;
    }

    public ITerm Left 
    {
        get => _left;
        private set => _left = value ?? throw new ArgumentNullException(nameof(Left));
    }

    public ArithmeticOperation Operation
    {
        get => _operation;
        private set => _operation = value ?? throw new ArgumentNullException(nameof(Operation));
    }

    public ITerm Right {
        get => _right;
        private set => _right = value ?? throw new ArgumentNullException(nameof(Right));
    }

    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"{_left} {_operation} {_right}";
    }
}
﻿using asp_interpreter_lib.Types.Terms;
using System.Data;

namespace asp_interpreter_lib.Types.BinaryOperations;

public class BinaryOperation
{
    private Term _left;
    private Term _right;
    private BinaryOperator _binaryOperator;

    public BinaryOperation(Term left, BinaryOperator binaryOperator,Term right)
    {
        Left = left;
        Right = right;
        BinaryOperator = binaryOperator;
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

    public BinaryOperator BinaryOperator
    {
        get => _binaryOperator;
        set => _binaryOperator = value ?? throw new ArgumentNullException(nameof(BinaryOperator));
    }

    //Evaluate by delegating to the operator
    public bool Evaluate()
    {
        return BinaryOperator.Evaluate(Left, Right);
    }
    
    public override string ToString()
    {
        return $"{Left.ToString()} {BinaryOperator.ToString()} {Right.ToString()}";
    }
}
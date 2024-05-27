//-----------------------------------------------------------------------
// <copyright file="BinaryOperation.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.BinaryOperations;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class BinaryOperation : Goal
{
    private ITerm left;
    private ITerm right;
    private BinaryOperator binaryOperator;

    public BinaryOperation(ITerm left, BinaryOperator binaryOperator, ITerm right)
    {
        this.Left = left;
        this.Right = right;
        this.BinaryOperator = binaryOperator;
    }

    public ITerm Left
    {
        get => this.left;
        set => this.left = value ?? throw new ArgumentNullException(nameof(this.Left));
    }

    public ITerm Right
    {
        get => this.right;
        set => this.right = value ?? throw new ArgumentNullException(nameof(this.Right));
    }

    public BinaryOperator BinaryOperator
    {
        get => this.binaryOperator;
        set => this.binaryOperator = value ?? throw new ArgumentNullException(nameof(this.BinaryOperator));
    }

    public override string ToString()
    {
        return $"{this.Left.ToString()} {this.BinaryOperator.ToString()} {this.Right.ToString()}";
    }

    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}
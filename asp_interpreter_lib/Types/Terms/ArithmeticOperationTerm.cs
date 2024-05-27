//-----------------------------------------------------------------------
// <copyright file="ArithmeticOperationTerm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.ArithmeticOperations;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class ArithmeticOperationTerm : ITerm
{
    private ArithmeticOperation operation;
    private ITerm left;
    private ITerm right;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArithmeticOperationTerm"/> class.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="operation"></param>
    /// <param name="right"></param>
    public ArithmeticOperationTerm(ITerm left, ArithmeticOperation operation, ITerm right)
    {
        this.Left = left;
        this.Operation = operation;
        this.Right = right;
    }

    public ITerm Left
    {
        get => this.left;
        private set => this.left = value ?? throw new ArgumentNullException(nameof(this.Left));
    }

    public ArithmeticOperation Operation
    {
        get => this.operation;
        private set => this.operation = value ?? throw new ArgumentNullException(nameof(this.Operation));
    }

    public ITerm Right
    {
        get => this.right;
        private set => this.right = value ?? throw new ArgumentNullException(nameof(this.Right));
    }

    /// <inheritdoc/>
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{this.left} {this.operation} {this.right}";
    }
}
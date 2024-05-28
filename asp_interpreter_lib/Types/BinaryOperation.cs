//-----------------------------------------------------------------------
// <copyright file="BinaryOperation.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types
{
    using Asp_interpreter_lib.Types.BinaryOperations;
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Represents a binary operation.
    /// </summary>
    public class BinaryOperation : Goal
    {
        private ITerm left;

        private ITerm right;

        private BinaryOperator binaryOperator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperation"/> class.
        /// </summary>
        /// <param name="left">The left hand side of the operation.</param>
        /// <param name="binaryOperator">The operation to be applied to the terms.</param>
        /// <param name="right">The right hand side of the operation.</param>
        /// <exception cref="ArgumentNullException">If any of the given arguments is null.</exception>
        public BinaryOperation(ITerm left, BinaryOperator binaryOperator, ITerm right)
        {
            this.left = left ?? throw new ArgumentNullException(nameof(left));
            this.right = right ?? throw new ArgumentNullException(nameof(right));
            this.binaryOperator = binaryOperator ?? throw new ArgumentNullException(nameof(binaryOperator));
        }

        /// <summary>
        /// Gets or sets the left hand side of the operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the given value is null.</exception>
        public ITerm Left
        {
            get => this.left;
            set => this.left = value ?? throw new ArgumentNullException(nameof(this.Left));
        }

        /// <summary>
        /// Gets or sets the right hand side of the operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the given value is null.</exception>
        public ITerm Right
        {
            get => this.right;
            set => this.right = value ?? throw new ArgumentNullException(nameof(this.Right));
        }

        /// <summary>
        /// Gets or sets the binary operator to apply to the terms.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the given value is null.</exception>
        public BinaryOperator BinaryOperator
        {
            get => this.binaryOperator;
            set => this.binaryOperator = value ?? throw new ArgumentNullException(nameof(this.BinaryOperator));
        }

        /// <summary>
        /// Returns the string representation of the type.
        /// </summary>
        /// <returns>The string representation of the type.</returns>
        public override string ToString()
        {
            return $"{this.Left.ToString()} {this.BinaryOperator.ToString()} {this.Right.ToString()}";
        }

        /// <summary>
        /// Accepts a <see cref="TypeBaseVisitor{T}"/> and returns the result of the given operation.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <param name="visitor">The visitor to accept.</param>
        /// <returns>Either none if the visitor fails to execute the corresponding
        /// method or the result wrapped into an instance of <see cref="Some{T}"/>class.</returns>
        /// <exception cref="ArgumentNullException">If the visitor is null.</exception>
        public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }
    }
}
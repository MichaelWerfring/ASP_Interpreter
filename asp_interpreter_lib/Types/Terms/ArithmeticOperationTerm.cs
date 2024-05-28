//-----------------------------------------------------------------------
// <copyright file="ArithmeticOperationTerm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms
{
    using Asp_interpreter_lib.Types.ArithmeticOperations;
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Represents an arithmetic operation term.
    /// </summary>
    public class ArithmeticOperationTerm : ITerm
    {
        private ArithmeticOperation operation;

        private ITerm left;

        private ITerm right;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArithmeticOperationTerm"/> class.
        /// </summary>
        /// <param name="left">The left hand side of the operation.</param>
        /// <param name="operation">The operation to apply to the terms.</param>
        /// <param name="right">The right hand side of the operation.</param>
        /// <exception cref="ArgumentNullException">Is thrown if any of the arguments is null.</exception>
        public ArithmeticOperationTerm(ITerm left, ArithmeticOperation operation, ITerm right)
        {
            this.left = left ?? throw new ArgumentNullException(nameof(left));
            this.operation = operation ?? throw new ArgumentNullException(nameof(operation));
            this.right = right ?? throw new ArgumentNullException(nameof(right));
        }

        /// <summary>
        /// Gets the left term of the arithmetic operation.
        /// </summary>
        public ITerm Left
        {
            get => this.left;
        }

        /// <summary>
        /// Gets the operation to apply to the terms.
        /// </summary>
        public ArithmeticOperation Operation
        {
            get => this.operation;
        }

        /// <summary>
        /// Gets the right term of the arithmetic operation.
        /// </summary>
        public ITerm Right
        {
            get => this.right;
        }

        /// <summary>
        /// Accepts a <see cref="TypeBaseVisitor{T}"/> and returns the result of the given operation.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <param name="visitor">The visitor to accept.</param>
        /// <returns>Either none if the visitor fails to execute the corresponding
        /// method or the result wrapped into an instance of <see cref="Some{T}"/>class.</returns>
        /// <exception cref="ArgumentNullException">If the visitor is null.</exception>
        public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
            return visitor.Visit(this);
        }

        /// <summary>
        /// Returns the string representation of the type.
        /// </summary>
        /// <returns>The string representation of the type.</returns>
        public override string ToString()
        {
            return $"{this.left} {this.operation} {this.right}";
        }
    }
}
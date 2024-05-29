//-----------------------------------------------------------------------
// <copyright file="BinaryOperatorNegator.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors
{
    using Asp_interpreter_lib.Types.BinaryOperations;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// This visitor negates a binary operators.
    /// </summary>
    public class BinaryOperatorNegator : TypeBaseVisitor<BinaryOperator>
    {
        /// <summary>
        /// Negates a disunification.
        /// </summary>
        /// <param name="_">Any disunification.</param>
        /// <returns>An equality operation.</returns>
        public override IOption<BinaryOperator> Visit(Disunification _)
        {
            return new Some<BinaryOperator>(new Equality());
        }

        /// <summary>
        /// Negates an equality operation.
        /// </summary>
        /// <param name="_">Any equality.</param>
        /// <returns>A disunification.</returns>
        public override IOption<BinaryOperator> Visit(Equality _)
        {
            return new Some<BinaryOperator>(new Disunification());
        }

        /// <summary>
        /// Negates a greater or equal than operation.
        /// </summary>
        /// <param name="_">Any greater or equal than operation.</param>
        /// <returns>A less than operation.</returns>
        public override IOption<BinaryOperator> Visit(GreaterOrEqualThan _)
        {
            return new Some<BinaryOperator>(new LessThan());
        }

        /// <summary>
        /// Negates a greater than operation.
        /// </summary>
        /// <param name="_">Any greater than operation</param>
        /// <returns>A less or equal operation.</returns>
        public override IOption<BinaryOperator> Visit(GreaterThan _)
        {
            return new Some<BinaryOperator>(new LessOrEqualThan());
        }

        /// <summary>
        /// Negates a less or equal than operation.
        /// </summary>
        /// <param name="_">Any less than less than equal than operation.</param>
        /// <returns>A greater than operation.</returns>
        public override IOption<BinaryOperator> Visit(LessOrEqualThan _)
        {
            return new Some<BinaryOperator>(new GreaterThan());
        }

        /// <summary>
        /// Negates a less than operation.
        /// </summary>
        /// <param name="_">Any less than operation.</param>
        /// <returns>A greater or equal than operation.</returns>
        public override IOption<BinaryOperator> Visit(LessThan _)
        {
            return new Some<BinaryOperator>(new GreaterOrEqualThan());
        }

        /// <summary>
        /// Negates a logical and operation.
        /// </summary>
        /// <param name="_">Any is operation.</param>
        /// <returns>An is not operation.</returns>
        public override IOption<BinaryOperator> Visit(Is _)
        {
            return new Some<BinaryOperator>(new IsNot());
        }

        /// <summary>
        /// Negates a logical or operation.
        /// </summary>
        /// <param name="_">Any is not operation.</param>
        /// <returns>An is operation.</returns>
        public override IOption<BinaryOperator> Visit(IsNot _)
        {
            return new Some<BinaryOperator>(new Is());
        }
    }
}
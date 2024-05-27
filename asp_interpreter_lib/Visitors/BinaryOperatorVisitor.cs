//-----------------------------------------------------------------------
// <copyright file="BinaryOperatorVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Types.BinaryOperations;
    using Asp_interpreter_lib.Util.ErrorHandling;

    public class BinaryOperatorVisitor(ILogger logger) : ASPParserBaseVisitor<IOption<BinaryOperator>>
    {
        private readonly ILogger logger = logger ??
            throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

        /// <inheritdoc/>
        public override IOption<BinaryOperator> VisitEqualityOperation(ASPParser.EqualityOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new Equality());
        }

        /// <inheritdoc/>
        public override IOption<BinaryOperator> VisitDisunificationOperation(ASPParser.DisunificationOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new Disunification());
        }

        /// <inheritdoc/>
        public override IOption<BinaryOperator> VisitLessOperation(ASPParser.LessOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new LessThan());
        }

        /// <inheritdoc/>
        public override IOption<BinaryOperator> VisitGreaterOperation(ASPParser.GreaterOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new GreaterThan());
        }

        /// <inheritdoc/>
        public override IOption<BinaryOperator> VisitLessOrEqOperation(ASPParser.LessOrEqOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new LessOrEqualThan());
        }

        /// <inheritdoc/>
        public override IOption<BinaryOperator> VisitGreaterOrEqOperation(ASPParser.GreaterOrEqOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new GreaterOrEqualThan());
        }

        /// <inheritdoc/>
        public override IOption<BinaryOperator> VisitIsOperation(ASPParser.IsOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new Is());
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="BinaryOperationVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Util.ErrorHandling;

    public class BinaryOperationVisitor : ASPParserBaseVisitor<IOption<BinaryOperation>>
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperationVisitor"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public BinaryOperationVisitor(ILogger logger)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
        }

        public override IOption<BinaryOperation> VisitBinary_operation(ASPParser.Binary_operationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            var op = context.binary_operator().Accept(new BinaryOperatorVisitor(this.logger));
            var left = context.term(0).Accept(new TermVisitor(this.logger));
            var right = context.term(1).Accept(new TermVisitor(this.logger));

            if (op == null || !op.HasValue)
            {
                this.logger.LogError("Cannot parse binary operator!", context);
                return new None<BinaryOperation>();
            }

            if (left == null || !left.HasValue)
            {
                this.logger.LogError("Cannot parse left term!", context);
                return new None<BinaryOperation>();
            }

            if (right == null || !right.HasValue)
            {
                this.logger.LogError("Cannot parse right term!", context);
                return new None<BinaryOperation>();
            }

            return new Some<BinaryOperation>(new BinaryOperation(
                left.GetValueOrThrow(),
                op.GetValueOrThrow(),
                right.GetValueOrThrow()));
        }
    }
}
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

    /// <summary>
    /// Utility class for traversing the ANTLR parse tree and
    /// creating the internal representation of <see cref="BinaryOperator"/> class.
    /// </summary>
    public class BinaryOperatorVisitor : ASPParserBaseVisitor<IOption<BinaryOperator>>
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorVisitor"/> class.
        /// </summary>
        /// <param name="logger">Logger to display potential error messages.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the logger is null.</exception>
        public BinaryOperatorVisitor(ILogger logger)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
        }

        /// <summary>
        /// Converts the given context to the corresponding <see cref="BinaryOperator"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<BinaryOperator> VisitEqualityOperation(ASPParser.EqualityOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new Equality());
        }

        /// <summary>
        /// Converts the given context to the corresponding <see cref="BinaryOperator"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<BinaryOperator> VisitDisunificationOperation(ASPParser.DisunificationOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new Disunification());
        }

        /// <summary>
        /// Converts the given context to the corresponding <see cref="BinaryOperator"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<BinaryOperator> VisitLessOperation(ASPParser.LessOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new LessThan());
        }

        /// <summary>
        /// Converts the given context to the corresponding <see cref="BinaryOperator"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<BinaryOperator> VisitGreaterOperation(ASPParser.GreaterOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new GreaterThan());
        }

        /// <summary>
        /// Converts the given context to the corresponding <see cref="BinaryOperator"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<BinaryOperator> VisitLessOrEqOperation(ASPParser.LessOrEqOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new LessOrEqualThan());
        }

        /// <summary>
        /// Converts the given context to the corresponding <see cref="BinaryOperator"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<BinaryOperator> VisitGreaterOrEqOperation(ASPParser.GreaterOrEqOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new GreaterOrEqualThan());
        }

        /// <summary>
        /// Converts the given context to the corresponding <see cref="BinaryOperator"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<BinaryOperator> VisitIsOperation(ASPParser.IsOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<BinaryOperator>(new Is());
        }
    }
}
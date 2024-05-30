//-----------------------------------------------------------------------
// <copyright file="GoalVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Utility class for traversing the ANTLR parse tree and
    /// creating the internal representation of <see cref="Goal"/> class.
    /// </summary>
    public class GoalVisitor : ASPParserBaseVisitor<IOption<Goal>>
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoalVisitor"/> class.
        /// </summary>
        /// <param name="logger">Logger to display potential error messages.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the logger is null.</exception>
        public GoalVisitor(ILogger logger)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
        }

        /// <summary>
        /// Converts the given context to a <see cref="Goal"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<Goal> VisitGoal(ASPParser.GoalContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (context.children?.Count == 1)
            {
                return context.children.ElementAt(0).Accept(this);
            }

            this.logger.LogError("Cannot parse goal!", context);
            return new None<Goal>();
        }

        /// <summary>
        /// Converts the given binary operation context to a <see cref="Goal"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<Goal> VisitBinary_operation(ASPParser.Binary_operationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var visitor = new BinaryOperationVisitor(this.logger);

            var result = visitor.VisitBinary_operation(context);
            if (!result.HasValue)
            {
                this.logger.LogError("Cannot parse binary operation!", context);
                return new None<Goal>();
            }

            return new Some<Goal>(result.GetValueOrThrow());
        }

        /// <summary>
        /// Converts the given literal context to a <see cref="Goal"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<Goal> VisitLiteral(ASPParser.LiteralContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            var visitor = new LiteralVisitor(this.logger);

            var result = visitor.VisitLiteral(context);
            if (!result.HasValue)
            {
                this.logger.LogError("Cannot parse literal!", context);
                return new None<Goal>();
            }

            return new Some<Goal>(result.GetValueOrThrow());
        }
    }
}
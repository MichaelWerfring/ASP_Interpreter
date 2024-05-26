namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Util.ErrorHandling;

    public class GoalVisitor : ASPParserBaseVisitor<IOption<Goal>>
    {
        public GoalVisitor(ILogger logger)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
        }

        private readonly ILogger logger;

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
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Visitors
{
    public class StatementVisitor : ASPParserBaseVisitor<IOption<Statement>>
    {
        private readonly ILogger logger;

        public StatementVisitor(ILogger logger)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
        }

        public override IOption<Statement> VisitStatement(ASPParser.StatementContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var statement = new Statement();

            var head = context.literal()?.Accept(new LiteralVisitor(this.logger));
            head?.IfHasValue((value) => statement.AddHead(value));

            BinaryOperationVisitor binaryOperationVisitor = new(this.logger);

            var goals = context.goal();

            if (goals == null)
            {
                // The statement has no body, so its a fact
                return new Some<Statement>(statement);
            }

            var goalVisitor = new GoalVisitor(this.logger);
            List<Goal> body = [];

            foreach (var goal in goals)
            {

                var parsedGoal = goal.Accept(goalVisitor);

                if (parsedGoal == null)
                {
                    this.logger.LogError("Cannot parse goal.", context);
                    return new None<Statement>();
                }

                if (parsedGoal.HasValue)
                {
                    body.Add(parsedGoal.GetValueOrThrow());
                    continue;
                }

                goal.Accept(binaryOperationVisitor)?.IfHasValue(v => body.Add(v));
            }

            if (goals.Length != body.Count)
            {
                this.logger.LogError("Not all goals could be parsed correctly!", context);
                return new None<Statement>();
            }

            statement.AddBody(body);
            return new Some<Statement>(statement);
        }
    }
}
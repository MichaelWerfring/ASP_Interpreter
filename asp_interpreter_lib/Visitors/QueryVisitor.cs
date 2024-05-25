namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System.Reflection.Metadata.Ecma335;

    public class QueryVisitor : ASPParserBaseVisitor<IOption<Query>>
    {
        private readonly ILogger logger;

        private readonly GoalVisitor goalVisitor;

        public QueryVisitor(ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger);
            this.logger = logger;
            this.goalVisitor = new GoalVisitor(logger);
        }

        public override IOption<Query> VisitQuery(ASPParser.QueryContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            List<Goal> query = [];

            for (int i = 0; i < context.goal().Length; i++)
            {
                var goal = context.goal(i);

                if (goal == null)
                {
                    this.logger.LogError($"Cannot parse goal ${i + 1} of query!", context);
                    continue;
                }

                goal.Accept(this.goalVisitor).
                    IfHasValue(g => query.Add(g));
            }

            if (query.Count != context.goal().Length)
            {
                this.logger.LogError("Not all goals could be parsed correctly, invalid query specified!", context);
                return new None<Query>();
            }

            return new Some<Query>(new Query(query));
        }
    }
}
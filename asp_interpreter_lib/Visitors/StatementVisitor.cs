//-----------------------------------------------------------------------
// <copyright file="StatementVisitor.cs" company="FHWN">
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
    /// creating the internal representation of <see cref="Statement"/> class.
    /// </summary>
    public class StatementVisitor : ASPParserBaseVisitor<IOption<Statement>>
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementVisitor"/> class.
        /// </summary>
        /// <param name="logger">Logger to display potential error messages.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the logger is null.</exception>
        public StatementVisitor(ILogger logger)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
        }

        /// <summary>
        /// Converts the given context to a <see cref="Statement"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<Statement> VisitStatement(ASPParser.StatementContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var statement = new Statement();

            var head = context.literal()?.Accept(new LiteralVisitor(this.logger));
            head?.IfHasValue(statement.AddHead);

            BinaryOperationVisitor binaryOperationVisitor = new(this.logger);

            var goals = context.goal();

            if (goals == null)
            {
                // The statement has no body, so its a fact
                return new Some<Statement>(statement);
            }

            var goalVisitor = new GoalVisitor(this.logger);
            List<Goal> body = new List<Goal>();

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

                goal.Accept(binaryOperationVisitor)?.IfHasValue(body.Add);
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
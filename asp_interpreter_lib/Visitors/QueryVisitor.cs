//-----------------------------------------------------------------------
// <copyright file="QueryVisitor.cs" company="FHWN">
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
    /// creating the internal representation of <see cref="Query"/> class.
    /// </summary>
    public class QueryVisitor : ASPParserBaseVisitor<IOption<Query>>
    {
        private readonly ILogger logger;

        private readonly GoalVisitor goalVisitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryVisitor"/> class.
        /// </summary>
        /// <param name="logger">Logger to display potential error messages.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the logger is null.</exception>
        public QueryVisitor(ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger);
            this.logger = logger;
            this.goalVisitor = new GoalVisitor(logger);
        }

        /// <summary>
        /// Converts the given context to a <see cref="Query"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<Query> VisitQuery(ASPParser.QueryContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            List<Goal> query = new List<Goal>();

            for (int i = 0; i < context.goal().Length; i++)
            {
                var goal = context.goal(i);

                if (goal == null)
                {
                    this.logger.LogError($"Cannot parse goal ${i + 1} of query!", context);
                    continue;
                }

                goal.Accept(this.goalVisitor).
                    IfHasValue(query.Add);
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
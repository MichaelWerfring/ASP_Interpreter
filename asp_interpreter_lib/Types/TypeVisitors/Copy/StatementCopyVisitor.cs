//-----------------------------------------------------------------------
// <copyright file="StatementCopyVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors.Copy
{
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Provides utility methods for copying statements.
    /// </summary>
    public class StatementCopyVisitor : TypeBaseVisitor<Statement>
    {
        private readonly LiteralCopyVisitor literalCopyVisitor;

        private readonly GoalCopyVisitor goalCopyVisitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementCopyVisitor"/> class.
        /// </summary>
        public StatementCopyVisitor()
        {
            var termCopyVisitor = new TermCopyVisitor();
            this.literalCopyVisitor = new LiteralCopyVisitor(termCopyVisitor);
            this.goalCopyVisitor = new GoalCopyVisitor(termCopyVisitor);
        }

        /// <summary>
        /// Copies a statement.
        /// </summary>
        /// <param name="statement">The statement to be copied.</param>
        /// <returns>A copy of the statement.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if any goal of the statement cannot be copied.</exception>
        /// <exception cref="ArgumentNullException">Is thrown if the given statement is null.</exception>
        public override IOption<Statement> Visit(Statement statement)
        {
            ArgumentNullException.ThrowIfNull(statement);

            Statement copy = new();

            if (statement.HasHead)
            {
                statement.Head.GetValueOrThrow().Accept(this.literalCopyVisitor).IfHasValue(
                    copy.AddHead);
            }

            List<Goal> body = new List<Goal>();
            foreach (var goal in statement.Body)
            {
                goal.Accept(this.goalCopyVisitor).IfHasValue(body.Add);
            }

            if (body.Count != statement.Body.Count)
            {
                throw new InvalidOperationException("The body of the statement could not be copied!");
            }

            copy.AddBody(body);

            return new Some<Statement>(copy);
        }
    }
}
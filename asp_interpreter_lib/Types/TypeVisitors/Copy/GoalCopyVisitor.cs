//-----------------------------------------------------------------------
// <copyright file="GoalCopyVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors.Copy
{
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Util;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Provides utility methods for copying goals.
    /// </summary>
    public class GoalCopyVisitor : TypeBaseVisitor<Goal>
    {
        private readonly TermCopyVisitor termCopyVisitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoalCopyVisitor"/> class.
        /// </summary>
        /// <param name="termCopyVisitor">The visitor to copy terms in the goal.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the term visitor is null.</exception>
        public GoalCopyVisitor(TermCopyVisitor termCopyVisitor)
        {
            this.termCopyVisitor = termCopyVisitor ?? throw new ArgumentNullException(nameof(termCopyVisitor));
        }

        /// <summary>
        /// Copies a forall goal.
        /// </summary>
        /// <param name="goal">The goal to be copied.</param>
        /// <returns>A copy of the goal.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the given goal is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if the
        /// inner term cannot be copied.</exception>"
        public override IOption<Goal> Visit(Forall goal)
        {
            ArgumentNullException.ThrowIfNull(goal);

            var innerGoal = goal.Goal.Accept(this).GetValueOrThrow("The given goal cannot be copied!");
            var variable = new VariableTerm(goal.VariableTerm.Identifier.GetCopy());
            return new Some<Goal>(new Forall(variable, innerGoal));
        }

        /// <summary>
        /// Copies the literal goal.
        /// </summary>
        /// <param name="goal">The goal to be copied.</param>
        /// <returns>A copy of the goal.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the given goal is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if the terms in the literal
        /// cannot be copied correctly.</exception>
        public override IOption<Goal> Visit(Literal goal)
        {
            ArgumentNullException.ThrowIfNull(goal);
            bool naf = goal.HasNafNegation;
            bool classical = goal.HasStrongNegation;
            string identifier = goal.Identifier.GetCopy();
            List<ITerm> terms = new List<ITerm>();

            foreach (var term in goal.Terms)
            {
                term.Accept(this.termCopyVisitor).IfHasValue(t => { terms.Add(t); });
            }

            if (terms.Count != goal.Terms.Count)
            {
                throw new InvalidOperationException("Not all terms within the literal could be copied!");
            }

            return new Some<Goal>(new Literal(identifier, naf, classical, terms));
        }

        /// <summary>
        /// Copies a binary operation goal.
        /// </summary>
        /// <param name="binOp">The goal to be copied.</param>
        /// <returns>A copy of the binary operation.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the given goal is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if the left or right
        /// term cannot be read.</exception>"
        public override IOption<Goal> Visit(BinaryOperation binOp)
        {
            ArgumentNullException.ThrowIfNull(binOp);
            var leftCopy = binOp.Left.Accept(this.termCopyVisitor).GetValueOrThrow(
                "The given left term cannot be read!");
            var rightCopy = binOp.Right.Accept(this.termCopyVisitor).GetValueOrThrow(
                "The given right term cannot be read!");

            return new Some<Goal>(new BinaryOperation(leftCopy, binOp.BinaryOperator, rightCopy));
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="GoalNegator.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Preprocessing.DualRules
{
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Types.TypeVisitors.Copy;

    /// <summary>
    /// Provides utility method for negating goals.
    /// </summary>
    public class GoalNegator
    {
        /// <summary>
        /// Negates a given goal by either negating the literal or by wrapping it in an
        /// applicable combination of negation as failure and classical negation.
        /// </summary>
        /// <param name="goal">The goal to negated.</param>
        /// <param name="wrapInNot">A boolean value indicating whether the literal should just be negated or wrapped into negation literals.</param>
        /// <returns>The negated goal.</returns>
        /// <exception cref="ArgumentNullException">If the goal is null.</exception>
        public static Goal Negate(Goal goal, bool wrapInNot = false)
        {
            ArgumentNullException.ThrowIfNull(goal);

            var literal = goal.Accept(new GoalToLiteralConverter());

            if (!literal.HasValue)
            {
                // Convert goal to binary operation
                var binaryOperation = goal.Accept(new GoalToBinaryOperationConverter())
                    .GetValueOrThrow("The value must be either a literal or a binary operation!");

                // Create new Binary Operation and negate just the operator
                var newBinaryOperation = new BinaryOperation(
                    binaryOperation.Left,
                    binaryOperation.BinaryOperator.Accept(new BinaryOperatorNegator()).
                        GetValueOrThrow("Failed to negate binary operator!"),
                    binaryOperation.Right);

                return newBinaryOperation;
            }

            var actualLiteral = literal.GetValueOrThrow();

            // Copy the terms
            var terms =
                actualLiteral.Terms.Select(t => t.Accept(new TermCopyVisitor()).
                    GetValueOrThrow("Failed to parse term!")).ToList();

            if (!wrapInNot)
            {
                return new Literal(
                    actualLiteral.Identifier.ToString(),
                    !actualLiteral.HasNafNegation,
                    actualLiteral.HasStrongNegation,
                    terms);
            }

            bool naf = !actualLiteral.HasNafNegation;

            if (naf && actualLiteral.HasStrongNegation)
            {
                return new Literal("not", false, false,[new NegatedTerm(new ParenthesizedTerm(new BasicTerm(actualLiteral.Identifier.ToString(), terms)))]);
            }

            if (!naf && actualLiteral.HasStrongNegation)
            {
                return new Literal("-", false, false,[new BasicTerm(actualLiteral.Identifier.ToString(), terms)]);
            }

            if (naf && !actualLiteral.HasStrongNegation)
            {
                return new Literal("not", false, false,[new BasicTerm(actualLiteral.Identifier.ToString(), terms)]);
            }

            return new Literal(
                    actualLiteral.Identifier.ToString(),
                    false,
                    false,
                    terms);
        }
    }
}
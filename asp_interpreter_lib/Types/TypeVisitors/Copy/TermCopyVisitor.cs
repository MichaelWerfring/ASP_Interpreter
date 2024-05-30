//-----------------------------------------------------------------------
// <copyright file="TermCopyVisitor.cs" company="FHWN">
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
    /// Provides utility for copying terms.
    /// </summary>
    public class TermCopyVisitor : TypeBaseVisitor<ITerm>
    {
        /// <summary>
        /// Copies the given term.
        /// </summary>
        /// <param name="term">The term to copy.</param>
        /// <returns>A copy of the term.</returns>
        public override IOption<ITerm> Visit(AnonymousVariableTerm term)
        {
            return new Some<ITerm>(new AnonymousVariableTerm());
        }

        /// <summary>
        /// Copies the given term.
        /// </summary>
        /// <param name="term">The term to copy.</param>
        /// <returns>A copy of the term.</returns>
        public override IOption<ITerm> Visit(VariableTerm term)
        {
            return new Some<ITerm>(new VariableTerm(term.Identifier.GetCopy()));
        }

        /// <summary>
        /// Copies the given term.
        /// </summary>
        /// <param name="term">The term to copy.</param>
        /// <returns>A copy of the term.</returns>
        public override IOption<ITerm> Visit(ArithmeticOperationTerm term)
        {
            return new Some<ITerm>(new ArithmeticOperationTerm(term.Left, term.Operation, term.Right));
        }

        /// <summary>
        /// Copies the given term.
        /// </summary>
        /// <param name="term">The term to copy.</param>
        /// <returns>A copy of the term.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if the child term cannot be copied.</exception>
        public override IOption<ITerm> Visit(BasicTerm term)
        {
            List<ITerm> children = new List<ITerm>();

            term.Terms.ForEach(t =>
            {
                children.Add(t.Accept(this).GetValueOrThrow("The child term cannot be copied!"));
            });

            return new Some<ITerm>(new BasicTerm(term.Identifier.ToString(), children));
        }

        /// <summary>
        /// Copies the given term.
        /// </summary>
        /// <param name="term">The term to copy.</param>
        /// <returns>A copy of the term.</returns>
        public override IOption<ITerm> Visit(StringTerm term)
        {
            return new Some<ITerm>(new StringTerm(term.Value.GetCopy()));
        }

        /// <summary>
        /// Copies the given term.
        /// </summary>
        /// <param name="term">The term to copy.</param>
        /// <returns>A copy of the term.</returns>
        public override IOption<ITerm> Visit(NumberTerm term)
        {
            return new Some<ITerm>(new NumberTerm(term.Value));
        }

        /// <summary>
        /// Copies the given term.
        /// </summary>
        /// <param name="term">The term to copy.</param>
        /// <returns>A copy of the term.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if the inner term cannot be copied.</exception>
        public override IOption<ITerm> Visit(NegatedTerm term)
        {
            var innerTerm = term.Term.Accept(this);
            return new Some<ITerm>(new NegatedTerm(
                innerTerm.GetValueOrThrow("The inner term cannot be copied!")));
        }

        /// <summary>
        /// Copies the given term.
        /// </summary>
        /// <param name="term">The term to copy.</param>
        /// <returns>A copy of the term.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if the inner term cannot be copied.</exception>
        public override IOption<ITerm> Visit(ParenthesizedTerm term)
        {
            var innerTerm = term.Term.Accept(this);
            return new Some<ITerm>(new ParenthesizedTerm(
                innerTerm.GetValueOrThrow("The inner term cannot be copied!")));
        }

        /// <summary>
        /// Copies the given term.
        /// </summary>
        /// <param name="term">The term to copy.</param>
        /// <returns>A copy of the term.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if the head or tail cannot be copied.</exception>"
        public override IOption<ITerm> Visit(RecursiveList term)
        {
            ITerm head = term.Head.Accept(this).
                GetValueOrThrow("The head cannot be copied!");

            ITerm tail = term.Tail.Accept(this).
                GetValueOrThrow("The tail cannot be copied!");

            return new Some<ITerm>(new RecursiveList(head, tail));
        }

        /// <summary>
        /// Copies the given term.
        /// </summary>
        /// <param name="term">The term to copy.</param>
        /// <returns>A copy of the term.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if any of the lists terms cannot be copied.</exception>
        public override IOption<ITerm> Visit(ConventionalList term)
        {
            List<ITerm> children = new List<ITerm>();

            term.Terms.ForEach(t =>
            {
                children.Add(t.Accept(this).GetValueOrThrow("The child term cannot be parsed!"));
            });

            return new Some<ITerm>(new ConventionalList(children));
        }
    }
}
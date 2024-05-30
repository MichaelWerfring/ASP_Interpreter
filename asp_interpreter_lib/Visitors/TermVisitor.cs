//-----------------------------------------------------------------------
// <copyright file="TermVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Utility class for traversing the ANTLR parse tree and
    /// creating the internal representation of <see cref="ITerm"/> subclasses.
    /// </summary>
    public class TermVisitor : ASPParserBaseVisitor<IOption<ITerm>>
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TermVisitor"/> class.
        /// </summary>
        /// <param name="logger">Logger to display potential error messages.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the logger is null.</exception>
        public TermVisitor(ILogger logger)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
        }

        /// <summary>
        /// Converts the given context to a <see cref="NegatedTerm"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ITerm> VisitNegatedTerm(ASPParser.NegatedTermContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var baseTerm = context.term().Accept(this);

            if (!baseTerm.HasValue)
            {
                this.logger.LogError("Cannot parse inner term!", context);
                return new None<ITerm>();
            }

            return new Some<ITerm>(new NegatedTerm(baseTerm.GetValueOrThrow()));
        }

        /// <summary>
        /// Converts the given context to a <see cref="StringTerm"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ITerm> VisitStringTerm(ASPParser.StringTermContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var text = context.STRING().GetText();

            if (text == null)
            {
                this.logger.LogError("The string term must have a text!", context);
                return new None<ITerm>();
            }

            return new Some<ITerm>(new StringTerm(text[1..^1]));
        }

        /// <summary>
        /// Converts the given context to a <see cref="BasicTerm"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ITerm> VisitBasicTerm(ASPParser.BasicTermContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var id = context.ID().GetText();

            if (id == null)
            {
                this.logger.LogError("The term must have an identifier!", context);
                return new None<ITerm>();
            }

            var terms = context.terms();

            if (terms == null)
            {
                // Can be a basic term without any inner terms
                return new Some<ITerm>(new BasicTerm(id, new List<ITerm>()));
            }

            var innerTerms = terms.Accept(new TermsVisitor(this.logger));

            if (innerTerms == null || !innerTerms.HasValue)
            {
                this.logger.LogError("Cannot parse inner terms!", context);
                return new None<ITerm>();
            }

            return new Some<ITerm>(new BasicTerm(id, innerTerms.GetValueOrThrow()));
        }

        /// <summary>
        /// Converts the given context to a <see cref="ArithmeticOperationTerm"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ITerm> VisitArithmeticOperationTerm(ASPParser.ArithmeticOperationTermContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var left = context.term(0).Accept(this);
            var right = context.term(1).Accept(this);
            var operation = context.arithop().Accept(new ArithmeticOperationVisitor());

            if (!left.HasValue || !right.HasValue || !operation.HasValue)
            {
                this.logger.LogError("Cannot parse arithmetic operation!", context);
                return new None<ITerm>();
            }

            return new Some<ITerm>(new ArithmeticOperationTerm(
                left.GetValueOrThrow(),
                operation.GetValueOrThrow(),
                right.GetValueOrThrow()));
        }

        /// <summary>
        /// Converts the given context to a <see cref="ParenthesizedTerm"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ITerm> VisitParenthesizedTerm(ASPParser.ParenthesizedTermContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var baseTerm = context.term().Accept(this);

            if (!baseTerm.HasValue)
            {
                this.logger.LogError("Cannot parse inner term!", context);
                return new None<ITerm>();
            }

            return new Some<ITerm>(baseTerm.GetValueOrThrow());
        }

        /// <summary>
        /// Converts the given context to a <see cref="AnonymousVariableTerm"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ITerm> VisitAnonymousVariableTerm(ASPParser.AnonymousVariableTermContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ITerm>(new AnonymousVariableTerm());
        }

        /// <summary>
        /// Converts the given context to a <see cref="NumberTerm"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ITerm> VisitNumberTerm(ASPParser.NumberTermContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var textNumber = context.NUMBER().GetText();

            if (textNumber == null)
            {
                this.logger.LogError("Cannot find number!", context);
                return new None<ITerm>();
            }

            var isValidNumber = int.TryParse(textNumber, out var number);

            if (!isValidNumber)
            {
                this.logger.LogError("Term cannot be converted to number!", context);
                return new None<ITerm>();
            }

            return new Some<ITerm>(new NumberTerm(number));
        }

        /// <summary>
        /// Converts the given context to a <see cref="AnonymousVariableTerm"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ITerm> VisitVariableTerm(ASPParser.VariableTermContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var variable = context.VARIABLE().GetText();

            if (variable == null)
            {
                this.logger.LogError("Cannot parse name of the variable!", context);
                return new None<ITerm>();
            }

            return new Some<ITerm>(new VariableTerm(variable));
        }

        /// <summary>
        /// Converts the given context to a <see cref="ListTerm"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ITerm> VisitListTerm(ASPParser.ListTermContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var list = context.list().Accept(this);

            if (list == null || !list.HasValue)
            {
                this.logger.LogError("Cannot parse list!", context);
                return new None<ITerm>();
            }

            return new Some<ITerm>(list.GetValueOrThrow());
        }

        /// <summary>
        /// Converts the given context to a <see cref="ConventionalList"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ITerm> VisitConventionalList(ASPParser.ConventionalListContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var innerList = context.terms();

            if (innerList == null)
            {
                return new Some<ITerm>(new ConventionalList([]));
            }

            var terms = innerList.Accept(new TermsVisitor(this.logger));

            if (!terms.HasValue)
            {
                this.logger.LogError("Cannot parse list terms!", context);
                return new None<ITerm>();
            }

            return new Some<ITerm>(new ConventionalList(terms.GetValueOrThrow()));
        }

        /// <summary>
        /// Converts the given context to a <see cref="RecursiveList"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ITerm> VisitRecursiveList(ASPParser.RecursiveListContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var head = context.term(0).Accept(this);
            var tail = context.term(1).Accept(this);

            if (!head.HasValue)
            {
                this.logger.LogError("Cannot parse head term!", context);
                return new None<ITerm>();
            }

            if (!tail.HasValue)
            {
                this.logger.LogError("Cannot parse tail term!", context);
                return new None<ITerm>();
            }

            return new Some<ITerm>(new RecursiveList(head.GetValueOrThrow(), tail.GetValueOrThrow()));
        }
    }
}
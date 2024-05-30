//-----------------------------------------------------------------------
// <copyright file="LiteralVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Utility class for traversing the ANTLR parse tree and
    /// creating the internal representation of <see cref="Literal"/> class.
    /// </summary>
    public class LiteralVisitor : ASPParserBaseVisitor<IOption<Literal>>
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralVisitor"/> class.
        /// </summary>
        /// <param name="logger">Logger to display potential error messages.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the logger is null.</exception>
        public LiteralVisitor(ILogger logger)
        {
            this.logger = logger ??
            throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
        }

        /// <summary>
        /// Converts the given context to a <see cref="Literal"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<Literal> VisitLiteral(ASPParser.LiteralContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (context.ID() == null)
            {
                this.logger.LogError("Cannot parse literal due to missing identifier!", context);
                return new None<Literal>();
            }

            string id = context.ID().GetText();

            if (string.IsNullOrEmpty(id))
            {
                this.logger.LogError("Cannot parse literal due to missing or invalid identifier!", context);
                return new None<Literal>();
            }

            var terms = context.terms();
            bool hasClassicalNegation = context.MINUS() != null;
            bool hasNafNegation = context.NAF() != null;

            if (terms == null)
            {
                return new Some<Literal>(new Literal(id, hasNafNegation, hasClassicalNegation, new List<ITerm>()));
            }

            IOption<List<ITerm>> parsedTerms = terms.Accept(new TermsVisitor(this.logger));

            if (!parsedTerms.HasValue)
            {
                this.logger.LogError("Cannot parse terms of Literal {id}!", context);
                return new None<Literal>();
            }

            return new Some<Literal>(new Literal(
                id,
                hasNafNegation,
                hasClassicalNegation,
                parsedTerms.GetValueOrThrow()));
        }
    }
}
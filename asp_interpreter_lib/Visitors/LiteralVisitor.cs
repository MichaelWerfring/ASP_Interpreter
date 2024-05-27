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

    public class LiteralVisitor : ASPParserBaseVisitor<IOption<Literal>>
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralVisitor"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public LiteralVisitor(ILogger logger)
        {
            this.logger = logger ??
            throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
        }

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
                return new Some<Literal>(new Literal(id, hasNafNegation, hasClassicalNegation,[]));
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
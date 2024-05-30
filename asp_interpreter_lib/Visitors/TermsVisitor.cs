//-----------------------------------------------------------------------
// <copyright file="TermsVisitor.cs" company="FHWN">
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
    /// creating the internal representation of <see cref="TermsVisitor"/> class.
    /// </summary>
    public class TermsVisitor : ASPParserBaseVisitor<IOption<List<ITerm>>>
    {
        private readonly ILogger logger;

        private readonly TermVisitor termVisitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TermsVisitor"/> class.
        /// </summary>
        /// <param name="logger">Logger to display potential error messages.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the logger is null.</exception>
        public TermsVisitor(ILogger logger)
        {
            this.logger = logger ??
            throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
            this.termVisitor = new TermVisitor(logger);
        }

        /// <summary>
        /// Converts the given context to a list of <see cref="ITerm"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<List<ITerm>> VisitTerms(ASPParser.TermsContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var multipleTerms = context.terms();
            var singleTerm = context.term().Accept(this.termVisitor);

            if (!singleTerm.HasValue)
            {
                // This should never happen but if it does its a severe fail
                this.logger.LogError("Cannot parse value of compound term!", context);
                return new None<List<ITerm>>();
            }

            // End of recursion
            if (multipleTerms == null)
            {
                return new Some<List<ITerm>>([singleTerm.GetValueOrThrow()]);
            }

            var terms = multipleTerms.Accept(this);

            if (!terms.HasValue)
            {
                this.logger.LogError("Cannot parse value of compound term!", context);
                return new None<List<ITerm>>();
            }

            var list = terms.GetValueOrThrow();

            // To build the list in proper order
            list.Insert(0, singleTerm.GetValueOrThrow());
            return terms;
        }
    }
}
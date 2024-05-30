//-----------------------------------------------------------------------
// <copyright file="ExplanationTextVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Utility class for traversing the ANTLR parse tree and
    /// creating text for explanations.
    /// </summary>
    internal class ExplanationTextVisitor : ASPParserBaseVisitor<string>
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplanationTextVisitor"/> class.
        /// </summary>
        /// <param name="logger">Logger to display potential error messages.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the logger is null.</exception>
        public ExplanationTextVisitor(ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger);
            this.logger = logger;
        }

        /// <summary>
        /// Converts the given context to a text for explanations.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override string VisitExp_text(ASPParser.Exp_textContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            string text = context.GetText();

            if (string.IsNullOrEmpty(text))
            {
                this.logger.LogError("The explanation text cannot be parsed!", context);
                return string.Empty;
            }

            return text;
        }
    }
}
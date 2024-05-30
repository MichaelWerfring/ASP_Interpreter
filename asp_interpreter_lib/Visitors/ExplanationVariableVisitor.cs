//-----------------------------------------------------------------------
// <copyright file="ExplanationVariableVisitor.cs" company="FHWN">
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
    /// creating variables for explanations.
    /// </summary>
    internal class ExplanationVariableVisitor : ASPParserBaseVisitor<string>
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplanationVariableVisitor"/> class.
        /// </summary>
        /// <param name="logger">Logger to display potential error messages.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the logger is null.</exception>
        public ExplanationVariableVisitor(ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger);

            this.logger = logger;
        }

        /// <summary>
        /// Converts the given context to a variable name for explanations.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override string VisitExp_var(ASPParser.Exp_varContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (context.EXP_VAR() == null)
            {
                this.logger.LogError("Cannot parse Variable!", context);
                return string.Empty;
            }

            string text = context.EXP_VAR().GetText();

            if (string.IsNullOrEmpty(text))
            {
                this.logger.LogError("Cannot parse Variable!", context);
                return string.Empty;
            }

            return text;
        }
    }
}
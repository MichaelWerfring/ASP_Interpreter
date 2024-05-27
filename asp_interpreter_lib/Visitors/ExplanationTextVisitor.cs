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

    internal class ExplanationTextVisitor : ASPParserBaseVisitor<string>
    {
        private readonly ILogger logger;

        public ExplanationTextVisitor(ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger);
            this.logger = logger;
        }

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
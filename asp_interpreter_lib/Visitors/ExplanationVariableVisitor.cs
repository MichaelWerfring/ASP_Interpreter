namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Util.ErrorHandling;

    internal class ExplanationVariableVisitor : ASPParserBaseVisitor<string>
    {
        private readonly ILogger logger;

        public ExplanationVariableVisitor(ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(logger);

            this.logger = logger;
        }

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
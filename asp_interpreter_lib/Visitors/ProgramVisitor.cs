//-----------------------------------------------------------------------
// <copyright file="ProgramVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Util.ErrorHandling;

    public class ProgramVisitor : ASPParserBaseVisitor<IOption<AspProgram>>
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramVisitor"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public ProgramVisitor(ILogger logger)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
        }

        public override IOption<AspProgram> VisitProgram(ASPParser.ProgramContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            this.logger.LogInfo("Parsing program...");

            // Try getting the query
            var query = context.query()?.Accept(new QueryVisitor(this.logger));

            // Program does not need a query
            if (query == null)
            {
                this.logger.LogInfo("Program has been specified without a query.");
                query = new None<Query>();
            }

            // Parse the Statements
            var program = context.statements().children;

            if (program == null)
            {
                // Its still possible to have a query without any statements
                // The error message is just for clarification
                this.logger.LogInfo("Program has been specified without any statement.");
                return new Some<AspProgram>(new AspProgram([], query,[]));
            }

            Dictionary<(string, int), Explanation> explanations =[];
            List<Statement> statements =[];
            var statementVisitor = new StatementVisitor(this.logger);
            var explanationVisitor = new ExplanationVisitor(
                this.logger,
                new LiteralVisitor(this.logger));

            foreach (var statement in program)
            {
                var result = statement.Accept(statementVisitor);

                if (result != null && result.HasValue)
                {
                    statements.Add(result.GetValueOrThrow());
                    continue;
                }

                statement.Accept(explanationVisitor)?.IfHasValue(
                    v => explanations.Add((v.Literal.Identifier, v.Literal.Terms.Count), v));
            }

            return new Some<AspProgram>(new AspProgram(statements, query, explanations));
        }
    }
}
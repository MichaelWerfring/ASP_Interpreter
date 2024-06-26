﻿//-----------------------------------------------------------------------
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

    /// <summary>
    /// Utility class for traversing the ANTLR parse tree and
    /// creating the internal representation of <see cref="AspProgram"/> class.
    /// </summary>
    public class ProgramVisitor : ASPParserBaseVisitor<IOption<AspProgram>>
    {
        private readonly ILogger logger;

        private readonly ShowFlagVisitor flagVisitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramVisitor"/> class.
        /// </summary>
        /// <param name="logger">Logger to display potential error messages.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the logger is null.</exception>
        public ProgramVisitor(ILogger logger)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");
            this.flagVisitor = new ShowFlagVisitor(this.logger, new LiteralVisitor(this.logger));
        }

        /// <summary>
        /// Converts the given context to a <see cref="AspProgram"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
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

            // parse show flag
            var literalsToShow = new List<Literal>();
            foreach (var item in context.children)
            {
                var flag = item.Accept(this.flagVisitor);
                if (flag != null && flag.HasValue)
                {
                    literalsToShow.AddRange(flag.GetValueOrThrow());
                }
            }

            // Parse the Statements
            var program = context.statements().children;

            if (program == null)
            {
                // Its still possible to have a query without any statements
                // The error message is just for clarification
                this.logger.LogInfo("Program has been specified without any statement.");
                return new Some<AspProgram>(new AspProgram([], query, new Dictionary<(string Id, int Arity), Explanation>(), literalsToShow));
            }

            Dictionary<(string, int), Explanation> explanations = new Dictionary<(string, int), Explanation>();
            List<Statement> statements = new List<Statement>();
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

            return new Some<AspProgram>(new AspProgram(statements, query, explanations, literalsToShow));
        }
    }
}
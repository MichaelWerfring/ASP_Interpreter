//-----------------------------------------------------------------------
// <copyright file="TermVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Visits the show flag.
    /// </summary>
    internal class ShowFlagVisitor : ASPParserBaseVisitor<IOption<List<Literal>>>
    {
        private ILogger logger;

        private readonly ASPParserBaseVisitor<IOption<Literal>> literalVisitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowFlagVisitor"/> class.
        /// </summary>
        /// <param name="logger">The logger for the visitor.</param>
        /// <param name="literalVisitor">The visitor for literals in the flag.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the logger or visitor is null.</exception>
        public ShowFlagVisitor(ILogger logger, ASPParserBaseVisitor<IOption<Literal>> literalVisitor)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.literalVisitor = literalVisitor ?? throw new ArgumentNullException(nameof(literalVisitor));
        }

        /// <summary>
        /// Visits the show flag and parses its literals.
        /// </summary>
        /// <param name="context">The show flag context.</param>
        /// <returns>The literals in the flag.</returns>
        public override IOption<List<Literal>> VisitShowFlag(ASPParser.ShowFlagContext context)
        {
            if (context == null)
            {
                return new None<List<Literal>>();
            }

            List<Literal> result = new List<Literal>();

            foreach (var item in context.literal())
            {
                var literal = item.Accept(this.literalVisitor);

                if (literal.HasValue)
                {
                    result.Add(literal.GetValueOrThrow());
                    continue;
                }

                this.logger.LogError("Failed to parse literals in flag!", context);
                return new None<List<Literal>>();
            }

            return new Some<List<Literal>>(result);
        }
    }
}
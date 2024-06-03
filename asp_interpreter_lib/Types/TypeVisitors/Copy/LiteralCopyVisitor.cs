//-----------------------------------------------------------------------
// <copyright file="LiteralCopyVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors.Copy
{
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Util;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Provides utility for copying literals.
    /// </summary>
    public class LiteralCopyVisitor : TypeBaseVisitor<Literal>
    {
        private TypeBaseVisitor<ITerm> termCopyVisitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralCopyVisitor"/> class.
        /// </summary>
        /// <param name="termCopyVisitor">A visitor to copy the literals terms.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the given termCopyVisitor is null.</exception></exception>
        public LiteralCopyVisitor(TypeBaseVisitor<ITerm> termCopyVisitor)
        {
            this.termCopyVisitor = termCopyVisitor ?? throw new ArgumentNullException(nameof(termCopyVisitor));
        }

        /// <summary>
        /// Visits a literal and copies it.
        /// </summary>
        /// <param name="literal">The literal to be copied.</param>
        /// <returns>A copy of the literal.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the given literal is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if not all terms
        /// in the literal can be copied.</exception>
        public override IOption<Literal> Visit(Literal literal)
        {
            ArgumentNullException.ThrowIfNull(literal);

            bool naf = literal.HasNafNegation;
            bool classical = literal.HasStrongNegation;
            string identifier = literal.Identifier.GetCopy();
            List<ITerm> terms = new List<ITerm>();

            foreach (var term in literal.Terms)
            {
                term.Accept(this.termCopyVisitor).IfHasValue(t => { terms.Add(t); });
            }

            if (terms.Count != literal.Terms.Count)
            {
                throw new InvalidOperationException("Not all terms within the literal could be copied!");
            }

            return new Some<Literal>(new Literal(identifier, naf, classical, terms));
        }
    }
}
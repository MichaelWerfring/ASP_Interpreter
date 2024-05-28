//-----------------------------------------------------------------------
// <copyright file="NegatedTerm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms
{
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Represents a negated term.
    /// </summary>
    public class NegatedTerm : ITerm
    {
        private ITerm term;

        /// <summary>
        /// Initializes a new instance of the <see cref="NegatedTerm"/> class.
        /// </summary>
        /// <param name="term">The term to be negated.</param>
        /// <exception cref="ArgumentNullException">If the given term is null.</exception>
        public NegatedTerm(ITerm term)
        {
            this.term = term ?? throw new ArgumentNullException(nameof(term));
        }

        /// <summary>
        /// Gets the term to be negated.
        /// </summary>
        public ITerm Term
        {
            get => this.term;
            private set => this.term = value ?? throw new ArgumentNullException(nameof(this.Term), "Term cannot be null!");
        }

        /// <summary>
        /// Accepts a <see cref="TypeBaseVisitor{T}"/> and returns the result of the given operation.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <param name="visitor">The visitor to accept.</param>
        /// <returns>Either none if the visitor fails to execute the corresponding
        /// method or the result wrapped into an instance of <see cref="Some{T}"/>class.</returns>
        /// <exception cref="ArgumentNullException">If the visitor is null.</exception>
        public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
            return visitor.Visit(this);
        }

        /// <summary>
        /// Returns the string representation of the type.
        /// </summary>
        /// <returns>The string representation of the type.</returns>
        public override string ToString()
        {
            return "-" + this.Term.ToString();
        }
    }
}
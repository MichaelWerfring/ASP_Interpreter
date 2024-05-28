//-----------------------------------------------------------------------
// <copyright file="Literal.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types
{
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Util;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System.Text;

    /// <summary>
    /// Represents a literal.
    /// </summary>
    public class Literal : Goal
    {
        private List<ITerm> terms;
        private string identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="Literal"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the literal.</param>
        /// <param name="hasNafNegation">A boolean value indicating whether the literal is negated with negation as failure.</param>
        /// <param name="hasStrongNegation">A boolean value indicating whether the literal is negated with classical negation.</param>
        /// <param name="terms">The terms of the literal.</param>
        /// <exception cref="ArgumentException">If the given identifier is null or a whitespace.</exception>
        /// <exception cref="ArgumentNullException">Is thrown if the given terms are null.</exception>
        public Literal(string identifier, bool hasNafNegation, bool hasStrongNegation, List<ITerm> terms)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
            ArgumentException.ThrowIfNullOrEmpty(identifier);
            ArgumentNullException.ThrowIfNull(terms);

            this.identifier = identifier;
            this.terms = terms;
            this.HasStrongNegation = hasStrongNegation;
            this.HasNafNegation = hasNafNegation;
        }

        /// <summary>
        /// Gets or sets the terms of the literal.
        /// </summary>
        /// <exception cref="ArgumentNullException">Is thrown if the given value is null.</exception>
        public List<ITerm> Terms
        {
            get => this.terms;
            set => this.terms = value ?? throw new ArgumentNullException(nameof(this.Terms));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the literal has a strong negation.
        /// </summary>
        public bool HasStrongNegation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the literal has a negation as failure.
        /// </summary>
        public bool HasNafNegation { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the literal.
        /// </summary>
        /// <exception cref="ArgumentException">If the given value is null, whitespace or empty.</exception>"
        public string Identifier
        {
            get => this.identifier;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value == string.Empty)
                {
                    throw new ArgumentException(
                        "The given Identifier must not be null, whitespace or empty!",
                        nameof(this.Identifier));
                }

                this.identifier = value;
            }
        }

        /// <summary>
        /// Returns the string representation of the type.
        /// </summary>
        /// <returns>The string representation of the type.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            if (this.HasNafNegation)
            {
                builder.Append("not ");
            }

            if (this.HasStrongNegation)
            {
                builder.Append('-');
            }

            builder.Append(this.Identifier);

            if (this.Terms.Count > 0)
            {
                builder.Append('(');
                builder.Append(this.Terms.ListToString());
                builder.Append(')');
            }

            return builder.ToString();
        }

        /// <summary>
        /// Accepts a <see cref="TypeBaseVisitor{T}"/> and returns the result of the given operation.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <param name="visitor">The visitor to accept.</param>
        /// <returns>Either none if the visitor fails to execute the corresponding
        /// method or the result wrapped into an instance of <see cref="Some{T}"/>class.</returns>
        /// <exception cref="ArgumentNullException">If the visitor is null.</exception>
        public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }
    }
}
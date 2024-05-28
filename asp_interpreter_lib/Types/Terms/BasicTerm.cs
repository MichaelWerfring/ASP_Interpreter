//-----------------------------------------------------------------------
// <copyright file="BasicTerm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms
{
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Util;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System.Text;

    /// <summary>
    /// Represents a basic term.
    /// </summary>
    public class BasicTerm : ITerm
    {
        private string identifier;

        private List<ITerm> terms;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicTerm"/> class.
        /// </summary>
        /// <param name="identifier">The terms identifier.</param>
        /// <param name="terms">The inner terms of the given basic term.</param>
        /// <exception cref="ArgumentException">If the given identifier is null or a whitespace.</exception>
        /// <exception cref="ArgumentNullException">If the given terms are null.</exception>"
        public BasicTerm(string identifier, List<ITerm> terms)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(identifier, nameof(identifier));
            ArgumentNullException.ThrowIfNull(terms, nameof(terms));

            this.identifier = identifier;
            this.terms = terms;
        }

        /// <summary>
        /// Gets or sets the identifier of the term.
        /// </summary>
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
        /// Gets the inner terms of the given basic term.
        /// </summary>
        public List<ITerm> Terms
        {
            get => this.terms;
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
            var builder = new StringBuilder();
            builder.Append(this.Identifier);

            if (this.Terms.Count > 0)
            {
                builder.Append('(');
                builder.Append(this.Terms.ListToString());
                builder.Append(')');
            }

            return builder.ToString();
        }
    }
}
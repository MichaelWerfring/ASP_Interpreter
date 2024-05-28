//-----------------------------------------------------------------------
// <copyright file="VariableTerm.cs" company="FHWN">
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
    /// Represents a variable term.
    /// </summary>
    public class VariableTerm : ITerm
    {
        private string identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableTerm"/> class.
        /// </summary>
        /// <param name="identifier">The variables identifier.</param>
        public VariableTerm(string identifier)
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Gets or sets the identifier of the variable.
        /// </summary>
        public string Identifier
        {
            get => this.identifier;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value == string.Empty)
                {
                    throw new ArgumentException(
                        "Identifier cannot be null, empty or whitespace!",
                        nameof(this.Identifier));
                }

                this.identifier = value;
            }
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
            return this.Identifier;
        }
    }
}
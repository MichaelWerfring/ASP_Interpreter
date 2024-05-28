﻿//-----------------------------------------------------------------------
// <copyright file="StringTerm.cs" company="FHWN">
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
    /// Represents a string.
    /// </summary>
    public class StringTerm : ITerm
    {
        private string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTerm"/> class.
        /// </summary>
        /// <param name="value">The strings value.</param>
        /// <exception cref="ArgumentNullException">If the given value is null.</exception>
        public StringTerm(string value)
        {
            this.value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets the strings value.
        /// </summary>
        public string Value
        {
            get => this.value;
            private set => this.value = value ?? throw new ArgumentNullException(nameof(this.Value), "Value cannot be null!");
        }

        /// <summary>
        /// Returns the string representation of the type.
        /// </summary>
        /// <returns>The string representation of the type.</returns>
        public override string ToString()
        {
            return "\"" + this.Value + "\"";
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
    }
}
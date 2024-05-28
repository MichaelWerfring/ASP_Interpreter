//-----------------------------------------------------------------------
// <copyright file="ConventionalList.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms
{
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System.Text;

    /// <summary>
    /// Represents a conventional list.
    /// </summary>
    public class ConventionalList : ListTerm
    {
        private List<ITerm> terms;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionalList"/> class.
        /// </summary>
        public ConventionalList()
        {
            this.terms = new List<ITerm>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionalList"/> class.
        /// </summary>
        /// <param name="terms">The terms contained in the list.</param>
        /// <exception cref="ArgumentNullException">If the given list of terms is null.</exception>
        public ConventionalList(List<ITerm> terms)
        {
            this.terms = terms ?? throw new ArgumentNullException(nameof(terms));
        }

        /// <summary>
        /// Gets the terms of the list.
        /// </summary>
        public List<ITerm> Terms
        {
            get
            {
                return this.terms;
            }

            private set
            {
                ArgumentNullException.ThrowIfNull(value);
                this.terms = value;
            }
        }

        /// <summary>
        /// Returns the string representation of the type.
        /// </summary>
        /// <returns>The string representation of the type.</returns>
        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append("[");

            for (int i = 0; i < this.Terms.Count; i++)
            {
                sb.Append(this.Terms[i].ToString());
                if (i < this.Terms.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append("]");

            return sb.ToString();
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
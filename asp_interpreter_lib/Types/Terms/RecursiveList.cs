//-----------------------------------------------------------------------
// <copyright file="RecursiveList.cs" company="FHWN">
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
    /// Represents a recursive list.
    /// </summary>
    public class RecursiveList : ListTerm
    {
        private readonly ITerm head;

        private readonly ITerm tail;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursiveList"/> class.
        /// </summary>
        /// <param name="head">The first element of the given list.</param>
        /// <param name="tail">A term representing all the other terms within the list.</param>
        /// <exception cref="ArgumentNullException">If the head or tail is null.</exception>"
        public RecursiveList(ITerm head, ITerm tail)
        {
            ArgumentNullException.ThrowIfNull(head);
            ArgumentNullException.ThrowIfNull(tail);

            this.head = head;
            this.tail = tail;
        }

        /// <summary>
        /// Gets the head of the list.
        /// </summary>
        public ITerm Head
        {
            get
            {
                return this.head;
            }
        }

        /// <summary>
        /// Gets the tail of the list.
        /// </summary>
        public ITerm Tail
        {
            get
            {
                return this.tail;
            }
        }

        /// <summary>
        /// Returns the string representation of the type.
        /// </summary>
        /// <returns>The string representation of the type.</returns>
        public override string ToString()
        {
            return $"[{this.head.ToString()}| {this.tail.ToString()}]";
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
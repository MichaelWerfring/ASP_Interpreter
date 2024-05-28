//-----------------------------------------------------------------------
// <copyright file="Statement.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types
{
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System.Text;

    /// <summary>
    /// Represents a statement.
    /// </summary>
    public class Statement : IVisitableType
    {
        /// <summary>
        /// Gets a value indicating whether the statement has a body.
        /// </summary>
        public bool HasBody => this.Body.Count != 0;

        /// <summary>
        /// Gets a value indicating whether the statement has a head.
        /// </summary>
        public bool HasHead => this.Head.HasValue;

        /// <summary>
        /// Gets or sets the head of the statement, which is none per default.
        /// </summary>
        public IOption<Literal> Head { get; set; } = new None<Literal>();

        /// <summary>
        /// Gets the body of the statement, which is an empty list per default.
        /// </summary>
        public List<Goal> Body { get; private set; } = new([]);

        /// <summary>
        /// Adds a head to the statement.
        /// </summary>
        /// <param name="head">The head to be added.</param>
        /// <exception cref="ArgumentException">Is thrown if the statement already has a head.</exception>
        /// <exception cref="ArgumentNullException">Is thrown if the head is null.</exception>
        public void AddHead(Literal head)
        {
            ArgumentNullException.ThrowIfNull(head);
            if (this.HasHead)
            {
                throw new ArgumentException("A statement can only have one head");
            }

            this.Head = new Some<Literal>(head);
        }

        /// <summary>
        /// Adds a body to the given statement.
        /// </summary>
        /// <param name="body">The body to be added to the statement.</param>
        /// <exception cref="ArgumentException">Is thrown if the statement already has a body.</exception>
        /// <exception cref="ArgumentNullException">Is thrown if the body is null.</exception>
        public void AddBody(List<Goal> body)
        {
            ArgumentNullException.ThrowIfNull(body);
            if (this.HasBody)
            {
                throw new ArgumentException("A statement can only have one body");
            }

            this.Body = body;
        }

        /// <summary>
        /// Returns the string representation of the type.
        /// </summary>
        /// <returns>The string representation of the type.</returns>
        public override string ToString()
        {
            if (this.HasBody && this.HasHead)
            {
                return $"{this.Head.GetValueOrThrow().ToString()} :- {this.GetBodyAsString()}.";
            }

            if (this.HasBody && !this.HasHead)
            {
                return ":- " + this.GetBodyAsString() + ".";
            }

            if (!this.HasBody && !this.HasHead)
            {
                return string.Empty;
            }

            return $"{this.Head.GetValueOrThrow().ToString()}.";
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

        private string GetBodyAsString()
        {
            var builder = new StringBuilder();

            if (this.Body.Count < 1)
            {
                return string.Empty;
            }

            builder.Append(this.Body[0].ToString());
            for (var index = 1; index < this.Body.Count; index++)
            {
                var goal = this.Body[index];
                builder.Append(", ");
                builder.Append(goal.ToString());
            }

            return builder.ToString();
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="ASPProgram.cs" company="FHWN">
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
    /// Represents an ASP program.
    /// </summary>
    public class AspProgram : IVisitableType
    {
        private List<Statement> statements;

        private IOption<Query> query;

        private Dictionary<(string Id, int Arity), Explanation> explanations;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspProgram"/> class.
        /// </summary>
        /// <param name="statements">The statements in the program.</param>
        /// <param name="query">The query of the program.</param>
        /// <param name="explanations">The explanations of the program.</param>
        /// <exception cref="ArgumentNullException">If the statements, query or explanations are null.</exception>"
        public AspProgram(List<Statement> statements, IOption<Query> query, Dictionary<(string Id, int Arity), Explanation> explanations)
        {
            this.statements = statements ?? throw new ArgumentNullException(nameof(statements));
            this.query = query ?? throw new ArgumentNullException(nameof(query));
            this.explanations = explanations ?? throw new ArgumentNullException(nameof(explanations));
        }

        /// <summary>
        /// Gets the statements in the program.
        /// </summary>
        public List<Statement> Statements
        {
            get => this.statements;
            private set => this.statements = value ?? throw new ArgumentNullException(nameof(this.Statements));
        }

        /// <summary>
        /// Gets the query of the program.
        /// </summary>
        public IOption<Query> Query
        {
            get => this.query;
            private set => this.query = value ?? throw new ArgumentNullException(nameof(this.Query));
        }

        /// <summary>
        /// Gets the explanations of the program.
        /// </summary>
        public Dictionary<(string Id, int Arity), Explanation> Explanations
        {
            get => this.explanations;
            private set => this.explanations = value ?? throw new ArgumentNullException(nameof(this.Explanations));
        }

        /// <summary>
        /// Returns the string representation of the type.
        /// </summary>
        /// <returns>The string representation of the type.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var statement in this.Statements)
            {
                builder.Append(statement.ToString());
                builder.AppendLine();
            }

            if (this.Query.HasValue)
            {
                builder.Append(this.Query.GetValueOrThrow().ToString());
            }

            builder.AppendLine();

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
        public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
            return visitor.Visit(this);
        }
    }
}
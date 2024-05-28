//-----------------------------------------------------------------------
// <copyright file="Forall.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types
{
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Util.ErrorHandling;

    /// <summary>
    /// Represents the internal structure of a forall goal.
    /// </summary>
    public class Forall : Goal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Forall"/> class.
        /// </summary>
        /// <param name="variableTerm">The variable, which is the first parameter of the goal.</param>
        /// <param name="goal">The goal to be the second parameter of the forall.</param>
        /// <exception cref="ArgumentNullException">If any of the given arguments is null.</exception>"
        public Forall(VariableTerm variableTerm, Goal goal)
        {
            this.VariableTerm = variableTerm ?? throw new ArgumentNullException(nameof(variableTerm));
            this.Goal = goal ?? throw new ArgumentNullException(nameof(goal));
        }

        /// <summary>
        /// Gets the variable term of the forall.
        /// </summary>
        public VariableTerm VariableTerm
        {
            get;
        }

        /// <summary>
        /// Gets the goal of the forall.
        /// </summary>
        public Goal Goal
        {
            get;
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

        /// <summary>
        /// Returns the string representation of the type.
        /// </summary>
        /// <returns>The string representation of the type.</returns>
        public override string ToString()
        {
            string s = string.Empty;
            s += "forall(" + this.VariableTerm.ToString() + ", ";
            return s + this.Goal.ToString() + ")";
        }
    }
}
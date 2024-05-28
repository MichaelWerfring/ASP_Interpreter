//-----------------------------------------------------------------------
// <copyright file="Explanation.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types
{
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an explanation for any literal in the program.
    /// </summary>
    public class Explanation
    {
        private readonly List<string> textParts;

        private readonly HashSet<int> variablesAt;

        private readonly Literal literal;

        /// <summary>
        /// Initializes a new instance of the <see cref="Explanation"/> class.
        /// </summary>
        /// <param name="textParts">The text of the explanation split into text and variables.</param>
        /// <param name="variablesAt">The indexes of the variables.</param>
        /// <param name="literal">The literal at the head of the explanation.</param>
        /// <exception cref="ArgumentNullException">Is thrown if any argument is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if any of the indexes is less than 0 or greater than the variables list.</exception>
        public Explanation(List<string> textParts, HashSet<int> variablesAt, Literal literal)
        {
            this.textParts = textParts ?? throw new ArgumentNullException(nameof(textParts));
            this.variablesAt = variablesAt ?? throw new ArgumentNullException(nameof(variablesAt));
            this.literal = literal ?? throw new ArgumentNullException(nameof(literal));

            if (variablesAt.Any(n => n > textParts.Count || n < 0))
            {
                throw new InvalidOperationException(
                    "The indexes of the variables must be between than the text parts length and 0!");
            }
        }

        /// <summary>
        /// Gets the text of the explanation split into text and variables.
        /// </summary>
        public List<string> TextParts
        {
            get => this.textParts;
        }

        /// <summary>
        /// Gets the indexes of the variables.
        /// </summary>
        public HashSet<int> VariablesAt
        {
            get => this.variablesAt;
        }

        /// <summary>
        /// Gets the literal at the head of the explanation.
        /// </summary>
        public Literal Literal
        {
            get => this.literal;
        }
    }
}
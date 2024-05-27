//-----------------------------------------------------------------------
// <copyright file="Explanation.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Explanation
    {
        private readonly List<string> textParts;

        private readonly HashSet<int> variablesAt;

        private readonly Literal literal;

        public Explanation(List<string> textParts, HashSet<int> variablesAt, Literal literal)
        {
            this.textParts = textParts ?? throw new ArgumentNullException(nameof(textParts));
            this.variablesAt = variablesAt ?? throw new ArgumentNullException(nameof(variablesAt));
            this.literal = literal ?? throw new ArgumentNullException(nameof(literal));

            if (variablesAt.Any(n => n > textParts.Count || n < 0))
            {
                throw new InvalidOperationException(
                    "The indices of the variables must be between than the text parts length and 0!");
            }
        }

        public List<string> TextParts { get => this.textParts; }

        public HashSet<int> VariablesAt { get => this.variablesAt; }

        public Literal Literal { get => this.literal; }
    }
}
//-----------------------------------------------------------------------
// <copyright file="PrefixOptions.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Preprocessing
{
    public class PrefixOptions
    {
        public PrefixOptions(
            string forallPrefix,
            string emptyHeadPrefix,
            string checkPrefix,
            string dualPrefix,
            string variablePrefix)
        {
            ArgumentException.ThrowIfNullOrEmpty(forallPrefix, nameof(forallPrefix));
            ArgumentException.ThrowIfNullOrEmpty(emptyHeadPrefix, nameof(emptyHeadPrefix));
            ArgumentException.ThrowIfNullOrEmpty(checkPrefix, nameof(checkPrefix));
            ArgumentException.ThrowIfNullOrEmpty(dualPrefix, nameof(dualPrefix));
            ArgumentException.ThrowIfNullOrEmpty(variablePrefix, nameof(variablePrefix));

            this.ForallPrefix = forallPrefix;
            this.EmptyHeadPrefix = emptyHeadPrefix;
            this.CheckPrefix = checkPrefix;
            this.DualPrefix = dualPrefix;
            this.VariablePrefix = variablePrefix;
        }

        public string ForallPrefix { get; }

        public string EmptyHeadPrefix { get; }

        public string CheckPrefix { get; }

        public string DualPrefix { get; }

        public string VariablePrefix { get; }
    }
}
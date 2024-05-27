//-----------------------------------------------------------------------
// <copyright file="VariableTerm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class VariableTerm : ITerm
{
    private string identifier;

    public VariableTerm(string identifier)
    {
        this.Identifier = identifier;
    }

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

    /// <inheritdoc/>
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.Identifier;
    }
}
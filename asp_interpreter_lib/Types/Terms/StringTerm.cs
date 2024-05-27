//-----------------------------------------------------------------------
// <copyright file="StringTerm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class StringTerm : ITerm
{
    private string value;

    /// <summary>
    /// Initializes a new instance of the <see cref="StringTerm"/> class.
    /// </summary>
    /// <param name="value"></param>
    public StringTerm(string value)
    {
        this.Value = value;
    }

    // Allow empty strings and whitespaces
    public string Value
    {
        get => this.value;
        private set => this.value = value ?? throw new ArgumentNullException(nameof(this.Value), "Value cannot be null!");
    }

    public override string ToString()
    {
        return "\"" + this.Value + "\"";
    }

    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}
//-----------------------------------------------------------------------
// <copyright file="NumberTerm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class NumberTerm : ITerm
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NumberTerm"/> class.
    /// </summary>
    /// <param name="value"></param>
    public NumberTerm(int value)
    {
        this.Value = value;
    }

    // according to the grammar, the value
    // of a number term allows only integer
    public int Value { get; private set; }

    /// <inheritdoc/>
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.Value.ToString();
    }
}
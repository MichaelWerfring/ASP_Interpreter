//-----------------------------------------------------------------------
// <copyright file="Left.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util.ErrorHandling.Either;

public class Left<TLeft, TRight> : IEither<TLeft, TRight>
{
    private readonly TLeft value;

    public Left(TLeft value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        this.value = value;
    }

    /// <inheritdoc/>
    public bool IsRight => false;

    /// <inheritdoc/>
    public TLeft GetLeftOrThrow()
    {
        return this.value;
    }

    /// <inheritdoc/>
    public TRight GetRightOrThrow()
    {
        throw new InvalidOperationException();
    }
}
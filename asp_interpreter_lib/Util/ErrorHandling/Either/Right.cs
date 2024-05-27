//-----------------------------------------------------------------------
// <copyright file="Right.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util.ErrorHandling.Either;
using System;

public class Right<TLeft, TRight> : IEither<TLeft, TRight>
{
    private readonly TRight value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Right{TLeft, TRight}"/> class.
    /// </summary>
    /// <param name="value"></param>
    public Right(TRight value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        this.value = value;
    }

    public bool IsRight => true;

    public TLeft GetLeftOrThrow()
    {
        throw new InvalidOperationException();
    }

    public TRight GetRightOrThrow()
    {
        return this.value;
    }
}
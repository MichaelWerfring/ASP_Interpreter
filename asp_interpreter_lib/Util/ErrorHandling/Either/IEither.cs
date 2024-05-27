//-----------------------------------------------------------------------
// <copyright file="IEither.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util.ErrorHandling.Either;

public interface IEither<TLeft, TRight>
{
    public bool IsRight { get; }

    public TLeft GetLeftOrThrow();

    public TRight GetRightOrThrow();
}
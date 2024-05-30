//-----------------------------------------------------------------------
// <copyright file="IEither.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util.ErrorHandling.Either
{
    /// <summary>
    /// Represents a type that can be either of type <typeparamref name="TLeft"/> or <typeparamref name="TRight"/>.
    /// </summary>
    /// <typeparam name="TLeft">The failure result.</typeparam>
    /// <typeparam name="TRight">The successful result type.</typeparam>
    public interface IEither<TLeft, TRight>
    {
        /// <summary>
        /// Gets a value indicating whether the instance contains valid data.
        /// </summary>
        public bool IsRight { get; }

        /// <summary>
        /// Gets the left value or throws an exception if the instance is successful.
        /// </summary>
        /// <returns>Data representing an error state.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the instance contains valid data.</exception>
        public TLeft GetLeftOrThrow();

        /// <summary>
        /// Gets the right value or throws an exception if the instance is a failure.
        /// </summary>
        /// <returns>Valid data.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the instance contains error data.</exception>
        public TRight GetRightOrThrow();
    }
}
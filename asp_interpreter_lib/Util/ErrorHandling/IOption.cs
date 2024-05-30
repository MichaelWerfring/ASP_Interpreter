//-----------------------------------------------------------------------
// <copyright file="IOption.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util.ErrorHandling
{
    /// <summary>
    /// Represents a value that may or may not have a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public interface IOption<T>
    {
        /// <summary>
        /// Gets a value indicating whether the instance has a value.
        /// </summary>
        bool HasValue { get; }

        /// <summary>
        /// Gets the value of the instance or throws an exception.
        /// </summary>
        /// <returns>The value of the instance if any.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if the instance has no value.</exception>
        T GetValueOrThrow();

        /// <summary>
        /// Gets the value of the instance or throws an exception with the given message.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        /// <returns>The value of the instance if any.</returns>
        /// <exception cref="ArgumentException">Is thrown if the message is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if the instance has no value.</exception>
        T GetValueOrThrow(string message);

        /// <summary>
        /// Executes the given action if the instance has a value.
        /// </summary>
        /// <param name="hasValue">The action to execute if the instance has a value.</param>
        void IfHasValue(Action<T> hasValue);

        /// <summary>
        /// Executes the given action if the instance has no value.
        /// </summary>
        /// <param name="hasNoValue">The action to execute if the instance has no value.</param>
        void IfHasNoValue(Action hasNoValue);

        /// <summary>
        /// Executes one of the given actions depending on whether the instance has a value.
        /// </summary>
        /// <param name="hasValue">The action to execute if the instance has a value.</param>
        /// <param name="hasNoValue">The action to execute if the instance has no value.</param>
        void IfHasValueElse(Action<T> hasValue, Action hasNoValue);
    }
}
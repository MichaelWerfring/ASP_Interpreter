//-----------------------------------------------------------------------
// <copyright file="None.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util.ErrorHandling
{
    /// <summary>
    /// Represents the failure result of an operation.
    /// </summary>
    /// <typeparam name="T">The type of the value that may be contained in the class.</typeparam>
    public class None<T> : IOption<T>
    {
        /// <summary>
        /// Gets a value indicating whether the instance has a value.
        /// </summary>
        public bool HasValue { get => false; }

        /// <summary>
        /// Gets the value of the instance or throws an exception.
        /// </summary>
        /// <returns>The value of the instance if any.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if the instance has no value.</exception>
        public T GetValueOrThrow()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the value of the instance or throws an exception with the given message.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        /// <returns>The value of the instance if any.</returns>
        /// <exception cref="ArgumentException">Is thrown if the message is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if the instance has no value.</exception>
        public T GetValueOrThrow(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            }

            throw new InvalidOperationException(message);
        }

        /// <summary>
        /// Executes the given action if the instance has a value.
        /// </summary>
        /// <param name="action">The action to execute if the instance has a value.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the action is null.</exception>"
        public void IfHasValue(Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(action);
        }

        /// <summary>
        /// Executes the given action if the instance has no value.
        /// </summary>
        /// <param name="action">The action to execute if the instance has no value.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the action is null.</exception>""
        public void IfHasNoValue(Action action)
        {
            ArgumentNullException.ThrowIfNull(action);
            action();
        }

        /// <summary>
        /// Executes one of the given actions depending on whether the instance has a value.
        /// </summary>
        /// <param name="hasValueAction">The action to execute if the instance has a value.</param>
        /// <param name="hasNoValueAction">The action to execute if the instance has no value.</param>
        /// <exception cref="ArgumentNullException">Is thrown if either action is null.</exception>"
        public void IfHasValueElse(Action<T> hasValueAction, Action hasNoValueAction)
        {
            ArgumentNullException.ThrowIfNull(hasValueAction);
            ArgumentNullException.ThrowIfNull(hasNoValueAction);
            hasNoValueAction();
        }
    }
}
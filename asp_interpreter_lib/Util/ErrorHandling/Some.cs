//-----------------------------------------------------------------------
// <copyright file="Some.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util.ErrorHandling
{
    /// <summary>
    /// Represents the successful result of an operation.
    /// </summary>
    /// <typeparam name="T">The type contained in the class.</typeparam>
    public class Some<T> : IOption<T>
    {
        private readonly T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Some{T}"/> class.
        /// </summary>
        /// <param name="value">The value of the successful operation.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the value is null.</exception>
        public Some(T value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            this.value = value;
        }

        /// <summary>
        /// Gets a value indicating whether the instance has a value.
        /// </summary>
        public bool HasValue
        {
            get => true;
        }

        /// <summary>
        /// Gets the value of the instance or throws an exception.
        /// </summary>
        /// <returns>The value of the instance if any.</returns>
        /// <exception cref="InvalidOperationException">Is thrown if the instance has no value.</exception>
        public T GetValueOrThrow()
        {
            return this.value;
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
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            }

            return this.value;
        }

        /// <summary>
        /// Executes the given action if the instance has a value.
        /// </summary>
        /// <param name="hasValue">The action to execute if the instance has a value.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the action is null.</exception>
        public void IfHasValue(Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            action(this.value);
        }

        /// <summary>
        /// Executes the given action if the instance has no value.
        /// </summary>
        /// <param name="hasNoValue">The action to execute if the instance has no value.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the action is null.</exception>
        public void IfHasNoValue(Action hasNoValue)
        {
            ArgumentNullException.ThrowIfNull(hasNoValue);
        }

        /// <summary>
        /// Executes one of the given actions depending on whether the instance has a value.
        /// </summary>
        /// <param name="hasValueAction">The action to execute if the instance has a value.</param>
        /// <param name="hasNoValueAction">The action to execute if the instance has no value.</param>
        /// <exception cref="ArgumentNullException">Is thrown if either action is null.</exception>
        public void IfHasValueElse(Action<T> hasValueAction, Action hasNoValueAction)
        {
            ArgumentNullException.ThrowIfNull(hasValueAction);
            ArgumentNullException.ThrowIfNull(hasNoValueAction);

            hasValueAction(this.value);
        }
    }
}
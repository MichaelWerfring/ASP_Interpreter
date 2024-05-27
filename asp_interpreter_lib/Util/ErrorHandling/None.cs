//-----------------------------------------------------------------------
// <copyright file="None.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util.ErrorHandling
{
    public class None<T> : IOption<T>
    {
        /// <inheritdoc/>
        public bool HasValue { get => false; }

        /// <inheritdoc/>
        public T GetValueOrThrow()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public T GetValueOrThrow(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            }

            throw new InvalidOperationException(message);
        }

        /// <inheritdoc/>
        public void IfHasValue(Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(action);
        }

        /// <inheritdoc/>
        public void IfHasNoValue(Action action)
        {
            ArgumentNullException.ThrowIfNull(action);
            action();
        }

        /// <inheritdoc/>
        public void IfHasValueElse(Action<T> hasValueAction, Action hasNoValueAction)
        {
            ArgumentNullException.ThrowIfNull(hasValueAction);
            ArgumentNullException.ThrowIfNull(hasNoValueAction);
            hasNoValueAction();
        }
    }
}
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
        public bool HasValue { get => false; }

        public T GetValueOrThrow()
        {
            throw new InvalidOperationException();
        }

        public T GetValueOrThrow(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            }

            throw new InvalidOperationException(message);
        }

        public void IfHasValue(Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(action);
        }

        public void IfHasNoValue(Action action)
        {
            ArgumentNullException.ThrowIfNull(action);
            action();
        }

        public void IfHasValueElse(Action<T> hasValueAction, Action hasNoValueAction)
        {
            ArgumentNullException.ThrowIfNull(hasValueAction);
            ArgumentNullException.ThrowIfNull(hasNoValueAction);
            hasNoValueAction();
        }
    }
}
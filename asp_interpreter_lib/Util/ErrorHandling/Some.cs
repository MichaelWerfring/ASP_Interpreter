//-----------------------------------------------------------------------
// <copyright file="Some.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Util.ErrorHandling;

public class Some<T> : IOption<T>
{
    private readonly T value;

    public Some(T value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        this.value = value;
    }

    /// <inheritdoc/>
    public bool HasValue
    {
        get => true;
    }

    /// <inheritdoc/>
    public T GetValueOrThrow()
    {
        return this.value;
    }

    /// <inheritdoc/>
    public T GetValueOrThrow(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
        }

        return this.value;
    }

    /// <inheritdoc/>
    public void IfHasValue(Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        action(this.value);
    }

    /// <inheritdoc/>
    public void IfHasNoValue(Action hasNoValue)
    {
        ArgumentNullException.ThrowIfNull(hasNoValue);
    }

    /// <inheritdoc/>
    public void IfHasValueElse(Action<T> hasValueAction, Action hasNoValueAction)
    {
        ArgumentNullException.ThrowIfNull(hasValueAction);
        ArgumentNullException.ThrowIfNull(hasNoValueAction);

        hasValueAction(this.value);
    }
}
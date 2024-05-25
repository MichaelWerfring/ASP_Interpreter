namespace Asp_interpreter_lib.Util.ErrorHandling;

public class Some<T> : IOption<T>
{
    private T value;

    public Some(T value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        this.value = value;
    }

    public bool HasValue
    {
        get => true;
    }

    public T GetValueOrThrow()
    {
        return this.value;
    }

    public T GetValueOrThrow(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
        }

        return this.value;
    }

    public void IfHasValue(Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        action(this.value);
    }

    public void IfHasNoValue(Action hasNoValue)
    {
        ArgumentNullException.ThrowIfNull(hasNoValue);
    }

    public void IfHasValueElse(Action<T> hasValueAction, Action hasNoValueAction)
    {
        ArgumentNullException.ThrowIfNull(hasValueAction);
        ArgumentNullException.ThrowIfNull(hasNoValueAction);

        hasValueAction(this.value);
    }
}
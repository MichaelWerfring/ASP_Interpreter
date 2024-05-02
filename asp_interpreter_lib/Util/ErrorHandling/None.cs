namespace asp_interpreter_lib.Util.ErrorHandling;

public class None<T> : IOption<T>
{
    public bool HasValue => false;

    public T GetValueOrThrow() => throw new InvalidOperationException();
    public T GetValueOrThrow(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));

        throw new InvalidOperationException(message);
    }

    public void IfHasValue(Action<T> hasValue) { }

    public void IfHasNoValue(Action hasNoValue) => hasNoValue();

    public void IfHasValueElse(Action<T> hasValue, Action hasNoValue) => hasNoValue();
}
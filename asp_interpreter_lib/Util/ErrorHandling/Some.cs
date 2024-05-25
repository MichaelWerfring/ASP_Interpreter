namespace Asp_interpreter_lib.Util.ErrorHandling;

public class Some<T> : IOption<T>
{
    private T _value;
    
    public Some(T value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        _value = value;
    }
    
    public bool HasValue => true;


    public T GetValueOrThrow() => _value;
    public T GetValueOrThrow(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));

        return _value;
    }

    public void IfHasValue(Action<T> hasValue) => hasValue(_value);

    public void IfHasNoValue(Action hasNoValue) { }

    public void IfHasValueElse(Action<T> hasValue, Action hasNoValue) => hasValue(_value); 
}
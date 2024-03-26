namespace asp_interpreter_lib.ErrorHandling;

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

    public void IfHasValue(Action<T> hasValue) => hasValue(_value);

    public void IfHasNoValue(Action hasNoValue) { }

    public void IfHasValueElse(Action<T> hasValue, Action hasNoValue) => hasValue(_value); 
}
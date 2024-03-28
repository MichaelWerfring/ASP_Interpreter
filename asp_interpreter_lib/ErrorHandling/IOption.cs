namespace asp_interpreter_lib.ErrorHandling;

public interface IOption<T>
{
    bool HasValue { get; }
    
    T GetValueOrThrow();
    
    void IfHasValue(Action<T> hasValue);
    
    void IfHasNoValue(Action hasNoValue);
    
    void IfHasValueElse(Action<T> hasValue, Action hasNoValue);
}
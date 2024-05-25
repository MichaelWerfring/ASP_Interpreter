namespace Asp_interpreter_lib.Util.ErrorHandling;

public interface IOption<T>
{
    bool HasValue { get; }
    
    T GetValueOrThrow();

    T GetValueOrThrow(string message);
    
    void IfHasValue(Action<T> hasValue);
    
    void IfHasNoValue(Action hasNoValue);
    
    void IfHasValueElse(Action<T> hasValue, Action hasNoValue);
}
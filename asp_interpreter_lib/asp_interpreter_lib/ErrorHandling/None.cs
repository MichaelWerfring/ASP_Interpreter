namespace asp_interpreter_lib.ErrorHandling;

public class None<T> : IOption<T>
{
    public bool HasValue => false;

    public T GetValueOrThrow() => throw new InvalidOperationException();

    public void IfHasValue(Action<T> hasValue) { }

    public void IfHasNoValue(Action hasNoValue) => hasNoValue();

    public void IfHasValueElse(Action<T> hasValue, Action hasNoValue) => hasNoValue();
}
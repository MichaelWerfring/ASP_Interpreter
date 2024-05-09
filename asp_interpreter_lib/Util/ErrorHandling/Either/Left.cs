namespace asp_interpreter_lib.Util.ErrorHandling.Either;

public class Left<TLeft, TRight> : IEither<TLeft, TRight>
{
    private TLeft _value;

    public Left(TLeft value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        _value = value;
    }

    public bool IsRight => false;

    public TLeft GetLeftOrThrow()
    {
        return _value;
    }

    public TRight GetRightOrThrow()
    {
        throw new InvalidOperationException();
    }
}

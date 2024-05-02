namespace asp_interpreter_lib.ErrorHandling.Either;

public interface IEither<TLeft, TRight>
{
    public bool IsRight { get; }

    public TLeft GetLeftOrThrow();

    public TRight GetRightOrThrow();

}

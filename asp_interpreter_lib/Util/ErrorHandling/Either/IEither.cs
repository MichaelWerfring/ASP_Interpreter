namespace Asp_interpreter_lib.Util.ErrorHandling.Either;

public interface IEither<TLeft, TRight>
{
    public bool IsRight { get; }

    public TLeft GetLeftOrThrow();

    public TRight GetRightOrThrow();

}

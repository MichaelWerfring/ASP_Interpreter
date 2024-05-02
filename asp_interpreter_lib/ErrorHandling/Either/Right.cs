using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace asp_interpreter_lib.ErrorHandling.Either;

public class Right<TLeft, TRight> : IEither<TLeft, TRight>
{
    private TRight _value;

    public Right(TRight value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        _value = value;
    }

    public bool IsRight => true;

    public TLeft GetLeftOrThrow()
    {
        throw new InvalidOperationException();
    }

    public TRight GetRightOrThrow()
    {
        return _value;
    }
}

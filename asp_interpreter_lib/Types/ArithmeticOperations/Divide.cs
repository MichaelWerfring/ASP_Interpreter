﻿using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.ArithmeticOperations;

public class Divide : ArithmeticOperation
{
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return $"/";
    }
}
﻿using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.BinaryOperations;

public class LessOrEqualThan : BinaryOperator
{
    public override bool Evaluate(Term left, Term right)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "<=";
    }
}
﻿using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.BinaryOperations;

public class Equality(Term left, Term right) : BinaryOperation(left, right)
{
    public override bool Evaluate()
    {
        throw new NotImplementedException();
    }
}
﻿using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.BinaryOperations;

public class LessThan : BinaryOperator, IVisitableType
{
    public override bool Evaluate(ITerm left, ITerm right)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "<";
    }
    
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}
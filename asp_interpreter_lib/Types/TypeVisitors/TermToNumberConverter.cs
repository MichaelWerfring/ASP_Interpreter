//-----------------------------------------------------------------------
// <copyright file="TermToNumberConverter.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;

public class TermToNumberConverter : TypeBaseVisitor<int>
{
    public override IOption<int> Visit(BasicTerm term)
    {
        return new None<int>();
    }

    public override IOption<int> Visit(AnonymousVariableTerm term)
    {
        return new None<int>();
    }

    public override IOption<int> Visit(VariableTerm term)
    {
        return new None<int>();
    }

    public override IOption<int> Visit(ArithmeticOperationTerm term)
    {
        var left = term.Left.Accept(this);
        var right = term.Left.Accept(this);

        if (!left.HasValue || !right.HasValue)
        {
            return new None<int>();
        }

        return new Some<int>(term.Operation.Evaluate(left.GetValueOrThrow(), right.GetValueOrThrow()));
    }

    public override IOption<int> Visit(ParenthesizedTerm term)
    {
        var result = term.Term.Accept(this);

        if (result.HasValue)
        {
            return new Some<int>(result.GetValueOrThrow());
        }

        return new None<int>();
    }

    public override IOption<int> Visit(StringTerm term)
    {
        return new None<int>();
    }

    public override IOption<int> Visit(NegatedTerm term)
    {
        var result = term.Term.Accept(this);

        if (result.HasValue)
        {
            return new Some<int>(-result.GetValueOrThrow());
        }

        return new None<int>();
    }

    public override IOption<int> Visit(NumberTerm term)
    {
        return new Some<int>(term.Value);
    }
}
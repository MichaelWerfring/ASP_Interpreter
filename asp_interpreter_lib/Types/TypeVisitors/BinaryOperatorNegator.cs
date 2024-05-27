//-----------------------------------------------------------------------
// <copyright file="BinaryOperatorNegator.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Types.BinaryOperations;
using Asp_interpreter_lib.Util.ErrorHandling;

public class BinaryOperatorNegator : TypeBaseVisitor<BinaryOperator>
{
    /// <inheritdoc/>
    public override IOption<BinaryOperator> Visit(Disunification _)
    {
        return new Some<BinaryOperator>(new Equality());
    }

    /// <inheritdoc/>
    public override IOption<BinaryOperator> Visit(Equality _)
    {
        return new Some<BinaryOperator>(new Disunification());
    }

    /// <inheritdoc/>
    public override IOption<BinaryOperator> Visit(GreaterOrEqualThan _)
    {
        return new Some<BinaryOperator>(new LessThan());
    }

    /// <inheritdoc/>
    public override IOption<BinaryOperator> Visit(GreaterThan _)
    {
        return new Some<BinaryOperator>(new LessOrEqualThan());
    }

    /// <inheritdoc/>
    public override IOption<BinaryOperator> Visit(LessOrEqualThan _)
    {
        return new Some<BinaryOperator>(new GreaterThan());
    }

    /// <inheritdoc/>
    public override IOption<BinaryOperator> Visit(LessThan _)
    {
        return new Some<BinaryOperator>(new GreaterOrEqualThan());
    }

    /// <inheritdoc/>
    public override IOption<BinaryOperator> Visit(Is _)
    {
        return new Some<BinaryOperator>(new IsNot());
    }

    /// <inheritdoc/>
    public override IOption<BinaryOperator> Visit(IsNot _)
    {
        return new Some<BinaryOperator>(new Is());
    }
}
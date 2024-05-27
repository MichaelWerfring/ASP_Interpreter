﻿//-----------------------------------------------------------------------
// <copyright file="GoalToLiteralConverter.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class GoalToLiteralConverter : TypeBaseVisitor<Literal>
{
    public override IOption<Literal> Visit(Literal literal)
    {
        ArgumentNullException.ThrowIfNull(literal);
        return new Some<Literal>(literal);
    }
}
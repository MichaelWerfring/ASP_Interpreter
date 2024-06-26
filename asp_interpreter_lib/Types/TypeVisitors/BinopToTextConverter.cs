﻿//-----------------------------------------------------------------------
// <copyright file="BinopToTextConverter.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors
{
    using Asp_interpreter_lib.Types.BinaryOperations;
    using Asp_interpreter_lib.Util.ErrorHandling;

    internal class BinopToTextConverter : TypeBaseVisitor<string>
    {
        public override IOption<string> Visit(GreaterOrEqualThan op)
        {
            return new Some<string>(" is greater or equal than ");
        }

        public override IOption<string> Visit(Equality op)
        {
            return new Some<string>(" is ");
        }

        public override IOption<string> Visit(Disunification op)
        {
            return new Some<string>(" is not ");
        }

        public override IOption<string> Visit(GreaterThan op)
        {
            return new Some<string>(" is greater than ");
        }

        public override IOption<string> Visit(LessOrEqualThan op)
        {
            return new Some<string>(" is less or equal than ");
        }

        public override IOption<string> Visit(LessThan op)
        {
            return new Some<string>(" is less than ");
        }
    }
}
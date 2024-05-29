﻿//-----------------------------------------------------------------------
// <copyright file="ArithmeticOperationVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Types.ArithmeticOperations;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System;

    /// <summary>
    /// Visits arithmetic operations.
    /// </summary>
    public class ArithmeticOperationVisitor : ASPParserBaseVisitor<IOption<ArithmeticOperation>>
    {
        /// <summary>
        /// Visit a plus context.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>The given operation.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>"
        public override IOption<ArithmeticOperation> VisitPlusOperation(ASPParser.PlusOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Plus());
        }

        /// <summary>
        /// Visit a minus context.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>The given operation.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>"
        public override IOption<ArithmeticOperation> VisitMinusOperation(ASPParser.MinusOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Minus());
        }

        /// <summary>
        /// Visit a multiply context.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>The given operation.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>"
        public override IOption<ArithmeticOperation> VisitTimesOperation(ASPParser.TimesOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Multiply());
        }

        /// <summary>
        /// Visit a divide context.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>The given operation.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>"
        public override IOption<ArithmeticOperation> VisitDivOperation(ASPParser.DivOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Divide());
        }

        /// <summary>
        /// Visit a power context.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>The given operation.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>"
        public override IOption<ArithmeticOperation> VisitPowerOperation(ASPParser.PowerOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Power());
        }
    }
}
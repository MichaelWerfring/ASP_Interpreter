//-----------------------------------------------------------------------
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
    /// Utility class for traversing the ANTLR parse tree and
    /// creating the internal representation of <see cref="ArithmeticOperation"/> class.
    /// </summary>
    public class ArithmeticOperationVisitor : ASPParserBaseVisitor<IOption<ArithmeticOperation>>
    {
        /// <summary>
        /// Converts the given context to the corresponding <see cref="ArithmeticOperation"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ArithmeticOperation> VisitPlusOperation(ASPParser.PlusOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Plus());
        }

        /// <summary>
        /// Converts the given context to the corresponding <see cref="ArithmeticOperation"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ArithmeticOperation> VisitMinusOperation(ASPParser.MinusOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Minus());
        }

        /// <summary>
        /// Converts the given context to the corresponding <see cref="ArithmeticOperation"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ArithmeticOperation> VisitTimesOperation(ASPParser.TimesOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Multiply());
        }

        /// <summary>
        /// Converts the given context to the corresponding <see cref="ArithmeticOperation"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ArithmeticOperation> VisitDivOperation(ASPParser.DivOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Divide());
        }

        /// <summary>
        /// Converts the given context to the corresponding <see cref="ArithmeticOperation"/>.
        /// </summary>
        /// <param name="context">Current parser context.</param>
        /// <returns>None if the context cannot be converted. Some if the conversion succeeds.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the context is null.</exception>
        public override IOption<ArithmeticOperation> VisitPowerOperation(ASPParser.PowerOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Power());
        }
    }
}
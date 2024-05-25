namespace Asp_interpreter_lib.Visitors
{
    using Asp_interpreter_lib.Types.ArithmeticOperations;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System;

    public class ArithmeticOperationVisitor : ASPParserBaseVisitor<IOption<ArithmeticOperation>>
    {
        public override IOption<ArithmeticOperation> VisitPlusOperation(ASPParser.PlusOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Plus());
        }

        public override IOption<ArithmeticOperation> VisitMinusOperation(ASPParser.MinusOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Minus());
        }

        public override IOption<ArithmeticOperation> VisitTimesOperation(ASPParser.TimesOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Multiply());
        }

        public override IOption<ArithmeticOperation> VisitDivOperation(ASPParser.DivOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Divide());
        }

        public override IOption<ArithmeticOperation> VisitPowerOperation(ASPParser.PowerOperationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return new Some<ArithmeticOperation>(new Power());
        }
    }
}
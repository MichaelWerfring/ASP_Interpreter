using System.Runtime.InteropServices;
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.BinaryOperations;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Visitors;

public class BinaryOperationVisitor(ILogger logger) : ASPParserBaseVisitor<IOption<BinaryOperation>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    public override IOption<BinaryOperation> VisitBinary_operation(ASPParser.Binary_operationContext context)
    {
        var op = context.binary_operator().Accept(new BinaryOperatorVisitor(_logger));
        var left = context.term(0).Accept(new TermVisitor(_logger));
        var right = context.term(1).Accept(new TermVisitor(_logger));

        if (op == null || !op.HasValue)
        {
            _logger.LogError("Cannot parse binary operator!", context);
            return new None<BinaryOperation>();
        }

        if (left == null || !left.HasValue)
        {
            _logger.LogError("Cannot parse left term!", context);
            return new None<BinaryOperation>();
        }

        if (right == null || !right.HasValue)
        {
            _logger.LogError("Cannot parse right term!", context);
            return new None<BinaryOperation>();
        }
        
        return new Some<BinaryOperation>(new BinaryOperation(
            left.GetValueOrThrow(), 
            op.GetValueOrThrow(),
            right.GetValueOrThrow()));
    }
}
using System.Runtime.InteropServices;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class BinaryOperationVisitor(ILogger logger) : ASPBaseVisitor<IOption<BinaryOperation>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    public override IOption<BinaryOperation> VisitBinary_operation(ASPParser.Binary_operationContext context)
    {
        var op = context.binary_operator().Accept(new BinaryOperatorVisitor(_logger));
        var left = context.term(0).Accept(new TermVisitor(_logger));
        var right = context.term(1).Accept(new TermVisitor(_logger));
        
        op.IfHasNoValue(() => _logger.LogError("Cannot parse binary operator!", context));
        left.IfHasNoValue(() => _logger.LogError("Cannot parse left term!", context));
        right.IfHasNoValue(() => _logger.LogError("Cannot parse right term!", context));

        if (!op.HasValue || !left.HasValue || !right.HasValue)
        {
            return new None<BinaryOperation>();   
        }
        
        return new Some<BinaryOperation>(new BinaryOperation(
            left.GetValueOrThrow(), 
            op.GetValueOrThrow(),
            right.GetValueOrThrow()));
    }
}
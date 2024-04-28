using System.Runtime.InteropServices;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class BinaryOperationVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<BinaryOperation>>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override IOption<BinaryOperation> VisitBinary_operation(ASPParser.Binary_operationContext context)
    {
        var op = context.binary_operator().Accept(new BinaryOperatorVisitor(_errorLogger));
        var left = context.term(0).Accept(new TermVisitor(_errorLogger));
        var right = context.term(1).Accept(new TermVisitor(_errorLogger));
        
        op.IfHasNoValue(() => _errorLogger.LogError("Cannot parse binary operator!", context));
        left.IfHasNoValue(() => _errorLogger.LogError("Cannot parse left term!", context));
        right.IfHasNoValue(() => _errorLogger.LogError("Cannot parse right term!", context));

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
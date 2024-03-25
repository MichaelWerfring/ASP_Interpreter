using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.BinaryOperations;

namespace asp_interpreter_lib.Visitors;

public class BinaryOperationVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<BinaryOperation>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override BinaryOperation VisitBinary_operation(ASPParser.Binary_operationContext context)
    {
        var op = context.binary_operator().Accept(new BinaryOperatorVisitor(_errorLogger));
        var left = context.term(0).Accept(new TermVisitor(_errorLogger));
        var right = context.term(1).Accept(new TermVisitor(_errorLogger));
        return new BinaryOperation(left,op, right);
    }
}
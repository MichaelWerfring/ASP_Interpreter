using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Visitors.TermVisitors;

namespace asp_interpreter_lib.Visitors;

public class BinaryOperationVisitor : ASPBaseVisitor<BinaryOperation>
{
    public override BinaryOperation VisitBinary_operation(ASPParser.Binary_operationContext context)
    {
        var op = context.binary_operator().Accept(new BinaryOperatorVisitor());
        var left = context.term(0).Accept(new TermVisitor());
        var right = context.term(1).Accept(new TermVisitor());
        return new BinaryOperation(left,op, right);
    }
}
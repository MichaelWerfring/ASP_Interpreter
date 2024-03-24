using asp_interpreter_lib.Types.ArithmeticOperations;

namespace asp_interpreter_lib.Visitors;

public class ArithmeticOperationVisitor : ASPBaseVisitor<ArithmeticOperation>
{
    public override ArithmeticOperation VisitArithop(ASPParser.ArithopContext context)
    {
        throw new NotImplementedException();
    }
}
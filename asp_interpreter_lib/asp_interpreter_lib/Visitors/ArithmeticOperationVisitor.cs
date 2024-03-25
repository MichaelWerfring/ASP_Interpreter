using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.ArithmeticOperations;

namespace asp_interpreter_lib.Visitors;

public class ArithmeticOperationVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<ArithmeticOperation>
{
    private IErrorLogger _errorLogger = errorLogger;

    public override ArithmeticOperation VisitArithop(ASPParser.ArithopContext context)
    {
        throw new NotImplementedException();
    }
}
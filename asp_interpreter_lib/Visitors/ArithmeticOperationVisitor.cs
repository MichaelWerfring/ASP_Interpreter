using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.ArithmeticOperations;

namespace asp_interpreter_lib.Visitors;

public class ArithmeticOperationVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<ArithmeticOperation>>
{
    private IErrorLogger _errorLogger = errorLogger;

    public override IOption<ArithmeticOperation> VisitArithop(ASPParser.ArithopContext context)
    {
        throw new NotImplementedException();
    }
}
using asp_interpreter_lib.Types.ArithmeticOperations;

namespace asp_interpreter_lib.Visitors;

public class ArithmeticOperationVisitor : ASPBaseVisitor<ArithmeticOperation>
{
    public override ArithmeticOperation VisitArithop(ASPParser.ArithopContext context)
    {
        var op = context.GetText() ?? string.Empty;
        return op switch
        {
            "+" => new Plus(),
            "-" => new Minus(),
            "*" => new Multiply(),
            "/" => new Dividie(),
            _ => throw new ArgumentException($"The given operation {op} is not supported!")
        };
    }
}
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.Terms;

public class ArithmeticOperationTerm : ITerm
{
    private ArithmeticOperation _operation;

    public ArithmeticOperationTerm(ArithmeticOperation operation)
    {
        Operation = operation;
    }

    public ArithmeticOperation Operation
    {
        get => _operation;
        private set => _operation = value ?? throw new ArgumentNullException(nameof(Operation));
    }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }

    public override string ToString()
    {
        return Operation.ToString();
    }
}
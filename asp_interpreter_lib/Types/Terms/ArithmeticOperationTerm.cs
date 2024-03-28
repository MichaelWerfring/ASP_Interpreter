using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.Terms;

public class ArithmeticOperationTerm : Term
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
    
    public override T Accept<T>(ITermVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
    
    public override void Accept(ITermVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }

    public override string ToString()
    {
        return Operation.ToString();
    }
}
using asp_interpreter_lib.Types.BinaryOperations;

namespace asp_interpreter_lib.Types;

public class NafLiteral
{
    private ClassicalLiteral _classicalLiteral;
    private BinaryOperation _binaryOperation;

    public NafLiteral(ClassicalLiteral literal, bool isNafNegated)
    {
        ClassicalLiteral = literal;
        IsClassicalLiteral = true;
        IsNafNegated = isNafNegated;
    }

    public NafLiteral(BinaryOperation binaryOperation)
    {
        IsBinaryOperation = true;
        BinaryOperation = binaryOperation;
    }
    
    public NafLiteral()
    {
    }

    public bool IsBinaryOperation { get; private set; }

    public bool IsClassicalLiteral { get; private set; }

    public bool IsNafNegated { get; private set; }

    public ClassicalLiteral ClassicalLiteral
    {
        get => _classicalLiteral;
        private set => _classicalLiteral = value ?? throw new ArgumentNullException(nameof(ClassicalLiteral));
    }

    public BinaryOperation BinaryOperation
    {
        get => _binaryOperation;
        private set => _binaryOperation = value ?? throw new ArgumentNullException(nameof(BinaryOperation));
    }
    
    public void AddClassicalLiteral(ClassicalLiteral literal, bool isNafNegated)
    {
        ArgumentNullException.ThrowIfNull(literal);
        
        if (IsBinaryOperation)
        {
            throw new InvalidOperationException("Cannot add a classical literal to a builtin atom!");
        }
        
        if (IsClassicalLiteral)
        {
            throw new InvalidOperationException("The classical literal has already been set!");
        }
        
        ClassicalLiteral = literal;
        IsNafNegated = isNafNegated;
        IsClassicalLiteral = true;
    }
    
    public void AddBinaryOperation(BinaryOperation binaryOperation)
    {
        ArgumentNullException.ThrowIfNull(binaryOperation);
        
        if (IsClassicalLiteral)
        {
            throw new InvalidOperationException("Cannot add a BinaryOperation to a classical literal!");
        }

        if (IsBinaryOperation)
        {
            throw new InvalidOperationException("The BinaryOperation has already been set!");
        }
        
        BinaryOperation = binaryOperation;
        IsBinaryOperation = true;
    }
}
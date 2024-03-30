using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.ArithmeticOperations;

public abstract class ArithmeticOperation
{
    private IVisitableType _left;
    
    private IVisitableType _right;
    
    protected ArithmeticOperation(IVisitableType left ,IVisitableType right)
    {
        Left = left;
        Right = right;
    }
    
    public abstract ITerm Evaluate();

    public IVisitableType Left
    {
        get => _left;
        private set => _left = value ?? throw new ArgumentNullException(nameof(Left));
    }

    public IVisitableType Right
    {
        get => _right;
        private set => _right = value ?? throw new ArgumentNullException(nameof(Right));
    }

    public abstract override string ToString();
}
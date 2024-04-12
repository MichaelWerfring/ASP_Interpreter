using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.ArithmeticOperations;

public abstract class ArithmeticOperation
{
    public abstract IOption<int> Evaluate(ITerm left, ITerm right);
    
    public abstract override string ToString();
}
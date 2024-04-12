using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.ArithmeticOperations;

public class Multiply : ArithmeticOperation
{
    public override IOption<int> Evaluate(ITerm left, ITerm right)
    {
        throw new NotImplementedException();
    }
    
    public override string ToString()
    {
        return $"*";
    }
}
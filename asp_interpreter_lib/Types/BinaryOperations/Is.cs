using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.BinaryOperations;

public class Is : BinaryOperator, IVisitableType
{
    public override bool Evaluate(ITerm left, ITerm right)
    {
        //Left term can only be a variable 
        //Right term must be arithmetic operation/evaluate to a number in the end
        throw new NotImplementedException();
    }
    
    public override string ToString()
    {
        return "is";
    }
    
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types.BinaryOperations;

public class Is : BinaryOperator, IVisitableType
{
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
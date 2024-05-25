using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types.TypeVisitors;

public interface IVisitableType
{
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
}
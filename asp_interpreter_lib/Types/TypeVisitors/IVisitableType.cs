using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.TypeVisitors;

public interface IVisitableType
{
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
}
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types;

public class Query
{
    private List<Goal> _goals;

    public Query(List<Goal> goals)
    {
        _goals = goals;
    }

    public List<Goal> Goals
    {
        get => _goals;
        private set => _goals = value ?? throw new ArgumentNullException(nameof(Goals));
    }

    public override string ToString()
    {
        return $"?- {AspExtensions.ListToString(Goals)}.";
    }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}
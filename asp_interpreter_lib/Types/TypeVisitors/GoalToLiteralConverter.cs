using asp_interpreter_lib.ErrorHandling;

namespace asp_interpreter_lib.Types.TypeVisitors;

public class GoalToLiteralConverter : TypeBaseVisitor<Literal>
{
    public override IOption<Literal> Visit(Literal literal)
    {
        ArgumentNullException.ThrowIfNull(literal);
        return new Some<Literal>(literal);
    }
}
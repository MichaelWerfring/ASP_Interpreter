using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Solving;

public class VariableTermVisitor : TypeBaseVisitor<VariableTerm>
{
    public override IOption<VariableTerm> Visit(VariableTerm term)
    {
        return new Some<VariableTerm>(term);
    }
}
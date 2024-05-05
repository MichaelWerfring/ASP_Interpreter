using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.TypeVisitors;

public class TermToBasicTermConverter : TypeBaseVisitor<BasicTerm>
{
    public override IOption<BasicTerm> Visit(BasicTerm term)
    {
        return new Some<BasicTerm>(term);
    }
}
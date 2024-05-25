using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types.TypeVisitors;

public class TermToBasicTermConverter : TypeBaseVisitor<BasicTerm>
{
    public override IOption<BasicTerm> Visit(BasicTerm term)
    {
        return new Some<BasicTerm>(term);
    }
}
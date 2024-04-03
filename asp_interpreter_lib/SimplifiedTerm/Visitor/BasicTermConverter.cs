using asp_interpreter_lib.ErrorHandling;

namespace asp_interpreter_lib.SimplifiedTerm.Visitor;

public class BasicTermConverter : ISimplifiedTermVisitor<IOption<BasicTerm>>
{
    public IOption<BasicTerm> Visit(VariableTerm variableTerm)
    {
        return new None<BasicTerm>();
    }

    public IOption<BasicTerm> Visit(BasicTerm basicTerm)
    {
        return new Some<BasicTerm>(basicTerm);
    }
}
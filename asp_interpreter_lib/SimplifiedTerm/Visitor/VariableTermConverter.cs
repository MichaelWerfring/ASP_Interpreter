using asp_interpreter_lib.ErrorHandling;

namespace asp_interpreter_lib.SimplifiedTerm.Visitor;

public class VariableTermConverter : ISimplifiedTermVisitor<IOption<VariableTerm>>
{
    public IOption<VariableTerm> Visit(VariableTerm variableTerm)
    {
        return new Some<VariableTerm>(variableTerm);
    }

    public IOption<VariableTerm> Visit(BasicTerm basicTerm)
    {
        return new None<VariableTerm>();
    }
}
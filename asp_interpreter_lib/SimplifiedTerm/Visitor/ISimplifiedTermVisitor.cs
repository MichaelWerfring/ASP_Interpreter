namespace asp_interpreter_lib.SimplifiedTerm.Visitor;

public interface ISimplifiedTermVisitor
{
    public void Visit(VariableTerm variableTerm);

    public void Visit(BasicTerm basicTerm);
}

public interface ISimplifiedTermVisitor<T>
{
    public T Visit(VariableTerm variableTerm);

    public T Visit(BasicTerm basicTerm);
}
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

public class TermBinding : IVariableBinding
{
    public TermBinding(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        Term = term;
    }

    public ISimpleTerm Term { get; }

    // visitor
    public void Accept(IVariableBindingVisitor visitor)
    {
        visitor.Visit(this);
    }

    public T Accept<T>(IVariableBindingVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public void Accept<TArgs>(IVariableBindingArgumentVisitor<TArgs> visitor, TArgs arguments)
    {
        visitor.Visit(this, arguments);
    }

    public TResult Accept<TResult, TArgs>(IVariableBindingArgumentVisitor<TResult, TArgs> visitor, TArgs arguments)
    {
        return visitor.Visit(this, arguments);
    }

    public override string ToString()
    {
        return Term.ToString();
    }
}

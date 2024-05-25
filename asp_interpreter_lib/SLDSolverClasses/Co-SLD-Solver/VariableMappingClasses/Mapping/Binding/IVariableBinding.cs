namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

public interface IVariableBinding
{
    public void Accept(IVariableBindingVisitor visitor);

    public T Accept<T>(IVariableBindingVisitor<T> visitor);

    public void Accept<TArgs>(IVariableBindingArgumentVisitor<TArgs> visitor, TArgs arguments);

    public TResult Accept<TResult, TArgs>(IVariableBindingArgumentVisitor<TResult,TArgs> visitor, TArgs arguments);
}

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

public interface IVariableBinding
{
    void Accept(IVariableBindingVisitor visitor);

    T Accept<T>(IVariableBindingVisitor<T> visitor);

    void Accept<TArgs>(IVariableBindingArgumentVisitor<TArgs> visitor, TArgs arguments);

    TResult Accept<TResult, TArgs>(IVariableBindingArgumentVisitor<TResult,TArgs> visitor, TArgs arguments);
}

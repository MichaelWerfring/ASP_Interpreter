using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

public class TermBindingTermBindingCase : IBinaryVariableBindingCase
{
    public TermBindingTermBindingCase(TermBinding left, TermBinding right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        Left = left;
        Right = right;
    }

    public TermBinding Left { get; }

    public TermBinding Right { get; }

    public void Accept(IBinaryVariableBindingCaseVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        visitor.Visit(this);
    }

    public T Accept<T>(IBinaryVariableBindingCaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        return visitor.Visit(this);
    }
}
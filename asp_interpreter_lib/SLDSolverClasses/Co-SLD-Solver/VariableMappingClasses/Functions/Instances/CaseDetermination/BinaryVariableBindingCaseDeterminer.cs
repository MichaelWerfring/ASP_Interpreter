using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.RightCaseDeterminers;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;

internal class BinaryVariableBindingCaseDeterminer : IVariableBindingArgumentVisitor<IBinaryVariableBindingCase, IVariableBinding>
{
    private readonly LeftIsProhibitedValuesCaseDeterminer _rightCaseDeterminerForProhibitedValuesCase = new();
    private readonly LeftIsTermBindingCaseDeterminer _rightCaseDeterminerForTermBindingCase = new();

    public IBinaryVariableBindingCase DetermineCase(IVariableBinding left, IVariableBinding right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left.Accept(this, right);
    }

    public IBinaryVariableBindingCase Visit(ProhibitedValuesBinding left, IVariableBinding right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(_rightCaseDeterminerForProhibitedValuesCase, left);
    }

    public IBinaryVariableBindingCase Visit(TermBinding left, IVariableBinding right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(_rightCaseDeterminerForTermBindingCase, left);
    }
}
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.RightCaseDeterminers;

internal class LeftIsProhibitedValuesCaseDeterminer : IVariableBindingArgumentVisitor<IBinaryVariableBindingCase, ProhibitedValuesBinding>
{
    public IBinaryVariableBindingCase Visit(ProhibitedValuesBinding right, ProhibitedValuesBinding left)
    {
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(left);

        return new ProhibValsProhibValsCase(left, right);
    }

    public IBinaryVariableBindingCase Visit(TermBinding right, ProhibitedValuesBinding left)
    {
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(left);

        return new ProhibValsTermBindingCase(left, right);
    }
}
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.RightCaseDeterminers;

internal class LeftIsTermBindingCaseDeterminer : IVariableBindingArgumentVisitor<IBinaryVariableBindingCase, TermBinding>
{
    public IBinaryVariableBindingCase Visit(ProhibitedValuesBinding right, TermBinding left)
    {
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(left);

        return new TermBindingProhibValsCase(left, right);
    }

    public IBinaryVariableBindingCase Visit(TermBinding right, TermBinding left)
    {
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(left);

        return new TermBindingTermBindingCase(left, right);
    }
}

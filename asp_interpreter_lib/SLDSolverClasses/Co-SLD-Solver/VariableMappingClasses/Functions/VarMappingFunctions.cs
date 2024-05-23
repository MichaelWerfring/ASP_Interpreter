using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.Splitter;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

internal static class VarMappingFunctions
{
    private static readonly VariableMappingMerger _merger = new();
    private static readonly BinaryVariableBindingCaseDeterminer _caseDeterminer = new();
    private static readonly ProhibitedValuesChecker _prohibFilterer = new();
    private static readonly TermBindingChecker _termbindingFilterer = new();

    public static VariableMapping Merge(this IDictionary<Variable, ProhibitedValuesBinding> prohibs, IDictionary<Variable, TermBinding> termbindings)
    {
        return _merger.Merge(prohibs, termbindings);
    }

    public static IBinaryVariableBindingCase DetermineCase(IVariableBinding left, IVariableBinding right)
    {
        return _caseDeterminer.DetermineCase(left, right);
    }

    public static IOption<ProhibitedValuesBinding> ReturnProhibitedValueBindingOrNone(this IVariableBinding variableBinding)
    {
        return _prohibFilterer.ReturnProhibitedValueBindingOrNone(variableBinding);
    }

    public static IOption<TermBinding> ReturnTermbindingOrNone(this IVariableBinding variableBinding)
    {
        return _termbindingFilterer.ReturnTermbindingOrNone(variableBinding);
    }
}

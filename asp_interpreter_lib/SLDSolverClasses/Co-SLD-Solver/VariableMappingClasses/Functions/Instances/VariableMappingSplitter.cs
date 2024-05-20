using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

public class VariableMappingSplitter
{
    public IImmutableDictionary<Variable, TermBinding> GetTermBindings(VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        return mapping
            .Where(pair => pair.Value is TermBinding)
            .Select(pair => (pair.Key, (TermBinding)pair.Value))
            .ToDictionary(TermFuncs.GetSingletonVariableComparer())
            .ToImmutableDictionary(TermFuncs.GetSingletonVariableComparer());
    }

    public IImmutableDictionary<Variable, ProhibitedValuesBinding> GetProhibitedValueBindings(VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        return mapping
            .Where(pair => pair.Value is ProhibitedValuesBinding)
            .Select(pair => (pair.Key, (ProhibitedValuesBinding)pair.Value))
            .ToDictionary(TermFuncs.GetSingletonVariableComparer())
            .ToImmutableDictionary(TermFuncs.GetSingletonVariableComparer());
    }
}

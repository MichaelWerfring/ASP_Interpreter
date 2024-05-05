using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

public class VariableMappingSplitter
{
    public (IImmutableDictionary<Variable, TermBinding>, IImmutableDictionary<Variable, ProhibitedValuesBinding>) Split(VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        var termBindings = mapping.Mapping
            .Where(pair => pair.Value is TermBinding)
            .Select(pair => (pair.Key, (TermBinding)pair.Value))
            .ToDictionary(new VariableComparer())
            .ToImmutableDictionary(new VariableComparer());

        var prohibitedValueBindings = mapping.Mapping
            .Where(pair => pair.Value is ProhibitedValuesBinding)
            .Select(pair => (pair.Key, (ProhibitedValuesBinding)pair.Value))
            .ToDictionary(new VariableComparer())
            .ToImmutableDictionary(new VariableComparer());

        return (termBindings, prohibitedValueBindings);
    }
}

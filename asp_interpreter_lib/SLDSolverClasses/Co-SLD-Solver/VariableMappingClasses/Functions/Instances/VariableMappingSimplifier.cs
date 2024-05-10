using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

internal class VariableMappingSimplifier
{
    private readonly BindingSimplifier _builder = new(false);

    public VariableMapping Simplify(VariableMapping variableMapping)
    {
        ArgumentNullException.ThrowIfNull(variableMapping, nameof(variableMapping));

        var vars = variableMapping.Mapping.Keys.ToArray();

        var newMapping = new (Variable, IVariableBinding)[vars.Length];

        Parallel.For(0, variableMapping.Mapping.Count, index =>
        {
            newMapping[index] = (vars[index], _builder.Build(variableMapping.Mapping[vars[index]], variableMapping));
        });

        return new VariableMapping(newMapping.ToDictionary(new VariableComparer()).ToImmutableDictionary(new VariableComparer()));
    }
}

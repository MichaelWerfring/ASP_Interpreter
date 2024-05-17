using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;

internal class VariableMappingFlattener
{
    private TransitiveVariableMappingResolver _resolver = new(false);

    public VariableMapping Simplify(VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        var newMapping = new KeyValuePair<Variable, IVariableBinding>[mapping.Count];

        Parallel.For(0, mapping.Count, index =>
        {
            var currentVariable = mapping.Keys.ElementAt(index);

            newMapping[index] = new KeyValuePair<Variable, IVariableBinding>
                (currentVariable, _resolver.Resolve(currentVariable, mapping));
        });

        return new VariableMapping(newMapping.ToImmutableDictionary(new VariableComparer()));
    }
}

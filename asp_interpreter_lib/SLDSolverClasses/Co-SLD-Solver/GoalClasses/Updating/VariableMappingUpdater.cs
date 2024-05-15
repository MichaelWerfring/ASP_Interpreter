using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;

internal class VariableMappingUpdater : IVariableBindingArgumentVisitor<IVariableBinding,VariableMapping>
{
    private readonly TransitiveVariableMappingResolver _resolver = new(true);

    public VariableMapping Update(VariableMapping left, VariableMapping right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        var newMapping = left
        .Select(pair =>
        {
            if (right.TryGetValue(pair.Key, out var value))
            {
                return new KeyValuePair<Variable, IVariableBinding>(pair.Key, value);
            }
            else
            {
                return new KeyValuePair<Variable, IVariableBinding>(pair.Key, pair.Value.Accept(this, right));
            }
        })
        .ToImmutableDictionary(new VariableComparer());

        return new VariableMapping(newMapping);
    }

    public IVariableBinding Visit(ProhibitedValuesBinding binding, VariableMapping map)
    {
        return binding;
    }

    public IVariableBinding Visit(TermBinding binding, VariableMapping map)
    {
        var resolvedValue = _resolver.Visit(binding, map);

        return resolvedValue;
    }
}

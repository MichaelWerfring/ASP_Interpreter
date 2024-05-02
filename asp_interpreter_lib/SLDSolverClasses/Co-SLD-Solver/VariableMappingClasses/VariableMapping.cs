using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

namespace asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

/// <summary>
/// Represents a mapping for the co-sld-resolution:
/// Variables are mapped to either a prohibited value set or a single term.
/// Wraps the dictionary to make sure it has the correct comparer.
/// </summary>
public class VariableMapping
{
    public VariableMapping(Dictionary<Variable, IVariableBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        if(mapping.Comparer is not VariableComparer)
        {
            throw new ArgumentException($"Comparer must be of type {typeof(VariableComparer)}");
        }

        Mapping = mapping;
    }

    public Dictionary<Variable, IVariableBinding> Mapping { get; }
}

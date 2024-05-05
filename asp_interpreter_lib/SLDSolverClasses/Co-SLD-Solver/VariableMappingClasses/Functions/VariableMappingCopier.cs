using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

public class VariableMappingCopier : IVariableBindingVisitor<IVariableBinding>
{
    /// <summary>
    /// Performs a deep copy of the mapping.
    /// </summary>
    /// <param name="mapping"></param>
    /// <returns>A deep copy of the mapping.</returns>
    public VariableMapping Copy(VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        var dict = mapping.Mapping
            .Select(pair => (new Variable(pair.Key.Identifier.GetCopy()), pair.Value.Accept(this)))
            .ToDictionary(new VariableComparer());

        var immutableBuilder = 
            ImmutableDictionary.CreateBuilder<Variable, IVariableBinding>(new VariableComparer());
        immutableBuilder.AddRange(dict);

        return new VariableMapping(immutableBuilder.ToImmutable());
    }

    public IVariableBinding Visit(ProhibitedValuesBinding binding)
    {
        return new ProhibitedValuesBinding
        (
            binding.ProhibitedValues.Select(x => x.Clone()).ToImmutableHashSet(new SimpleTermEqualityComparer())
        );
    }

    public IVariableBinding Visit(TermBinding binding)
    {
        return new TermBinding(binding.Term.Clone());
    }
}

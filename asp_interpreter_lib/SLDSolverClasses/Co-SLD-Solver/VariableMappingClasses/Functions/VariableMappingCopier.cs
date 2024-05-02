using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

public class VariableMappingCopier : IVariableBindingVisitor<IVariableBinding>
{
    private SimpleTermCloner _cloner = new SimpleTermCloner();

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
            .ToDictionary( new VariableComparer());

        return new VariableMapping(dict);
    }

    public IVariableBinding Visit(ProhibitedValuesBinding binding)
    {
        return new ProhibitedValuesBinding
        (
            binding.ProhibitedValues.Select(_cloner.Clone).ToHashSet(new SimpleTermComparer())
        );
    }

    public IVariableBinding Visit(TermBinding binding)
    {
        return new TermBinding(_cloner.Clone(binding.Term));
    }
}

using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling.Either;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;

public static class VariableMappingExtensions
{
    private static readonly VariableMappingSplitter _splitter = new();

    private static readonly VariableMappingSubstituter _substituter = new();

    public static IImmutableDictionary<Variable, TermBinding> GetTermBindings(this VariableMapping mapping)
    {
        return _splitter.GetTermBindings(mapping);
    }

    public static IImmutableDictionary<Variable, ProhibitedValuesBinding> GetProhibitedValueBindings(this VariableMapping mapping)
    {
        return _splitter.GetProhibitedValueBindings(mapping);
    }

    public static ISimpleTerm ApplySubstitution(this VariableMapping mapping, ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        return _substituter.Substitute(term, mapping);
    }
}

using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Concatenation.Exceptions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling.Either;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;

public static class VariableMappingExtensions
{
    private static VariableMappingCopier _copier = new VariableMappingCopier();
    private static VariableMappingSplitter _splitter = new VariableMappingSplitter();
    private static VariableMappingConcatenator _concatenator = new VariableMappingConcatenator();
    private static VariableMappingSubstituter _substituter = new VariableMappingSubstituter();

    public static VariableMapping Copy(this VariableMapping mapping)
    {
        return _copier.Copy(mapping);
    }

    public static (IImmutableDictionary<Variable, TermBinding>, IImmutableDictionary<Variable, ProhibitedValuesBinding>) Split(this VariableMapping mapping)
    {
        return _splitter.Split(mapping);
    }

    public static IEither<ConcatenationException, VariableMapping> Concatenate(this VariableMapping mapping, VariableMapping other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        return _concatenator.Concatenate(mapping, other);
    }

    public static ISimpleTerm ApplySubstitution(this VariableMapping mapping, ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        return _substituter.Substitute(term, mapping);
    }
}

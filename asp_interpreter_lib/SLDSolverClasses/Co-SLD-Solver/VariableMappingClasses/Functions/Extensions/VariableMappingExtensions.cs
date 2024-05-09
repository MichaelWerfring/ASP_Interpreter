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
    private static VariableMappingSplitter _splitter = new();
    private static VariableMappingUpdater _updater = new();
    private static VariableMappingSubstituter _substituter = new();

    public static (IImmutableDictionary<Variable, TermBinding>, IImmutableDictionary<Variable, ProhibitedValuesBinding>) Split(this VariableMapping mapping)
    {
        return _splitter.Split(mapping);
    }

    public static IEither<UpdateException, VariableMapping> Update(this VariableMapping mapping, VariableMapping other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        return _updater.Update(mapping, other);
    }

    public static ISimpleTerm ApplySubstitution(this VariableMapping mapping, ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        return _substituter.Substitute(term, mapping);
    }
}

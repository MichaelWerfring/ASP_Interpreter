using asp_interpreter_lib.FunctorNaming;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;

public static class SimpleTermExtensions
{
    private static readonly SimpleTermEnumerator _flattener = new();

    private static readonly SimpleTermComparer _comparer = new();

    private static readonly SimpleTermContainsChecker _containsChecker = new();

    private static readonly VariableSubstituter _variableSubstituter = new();

    private static readonly ClauseVariableRenamer _renamer = new(new(), new());

    private static readonly StructureReducer _reducer = new();

    public static bool IsEqualTo(this ISimpleTerm simpleTerm, ISimpleTerm other)
    {
        return _comparer.Compare(simpleTerm, other) == 0;
    }

    public static bool IsNegated(this ISimpleTerm term, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(functors);

        if
        (
            term is Structure structure
            && structure.Functor == functors.NegationAsFailure
            && structure.Children.Count == 1
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool Contains(this ISimpleTerm simpleTerm, ISimpleTerm other)
    {
        return _containsChecker.LeftContainsRight(simpleTerm, other);
    }

    public static int Compare(this ISimpleTerm left, ISimpleTerm other)
    {
        return _comparer.Compare(left, other);
    }

    public static ISimpleTerm Substitute(this ISimpleTerm simpleTerm, IDictionary<Variable, ISimpleTerm> substitution)
    {
        return _variableSubstituter.Substitute(simpleTerm, substitution);
    }

    public static ISimpleTerm NegateTerm(this ISimpleTerm term, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(functors);

        if 
        (
            term is Structure structure
            && structure.Functor == functors.NegationAsFailure
            && structure.Children.Count == 1
        )
        {
            return structure.Children.ElementAt(0);
        }
        else
        {
            return new Structure(functors.NegationAsFailure, [term]);
        }
    }

    public static RenamingResult RenameClause(this IEnumerable<ISimpleTerm> clause, int nextInternalIndex)
    {
        return _renamer.RenameVariables(clause, nextInternalIndex);
    }

    public static IEnumerable<Variable> ExtractVariables(this ISimpleTerm term)
    {
        return term.Enumerate().OfType<Variable>().ToImmutableHashSet(new VariableComparer());
    }

    public static IEnumerable<ISimpleTerm> Enumerate(this ISimpleTerm simpleTerm)
    {
        return _flattener.Enumerate(simpleTerm);
    }

    public static IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> Reduce(this IStructure structure, IStructure other)
    {
        return _reducer.TryReduce(structure, other);
    }
}

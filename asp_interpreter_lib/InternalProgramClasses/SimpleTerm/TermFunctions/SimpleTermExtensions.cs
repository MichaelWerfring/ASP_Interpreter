using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public static class SimpleTermExtensions
{
    private static readonly SimpleTermEnumerator _flattener = new();
    private static readonly SimpleTermComparer _comparer = new();
    private static readonly SimpleTermContainsChecker _containsChecker = new();
    private static readonly VariableSubstituter _variableSubstituter = new();

    public static bool IsEqualTo(this ISimpleTerm term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return _comparer.Compare(term, other) == 0;
    }

    public static bool IsNegated(this Structure term, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(functors);

        if 
        (
            term.Functor != functors.NegationAsFailure
            ||
            term.Children.Count != 1
            ||
            !TermFuncs.ReturnStructureOrNone(term.Children[0]).HasValue
        )
        {
            return false;
        }

        return true;
    }

    public static bool Contains(this ISimpleTerm term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return _containsChecker.LeftContainsRight(term, other);
    }

    public static int Compare(this ISimpleTerm term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return _comparer.Compare(term, other);
    }

    public static ISimpleTerm Substitute(this ISimpleTerm term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return _variableSubstituter.Substitute(term, map);
    }

    public static Structure Substitute(this Structure term, IDictionary<Variable, ISimpleTerm> map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return _variableSubstituter.SubsituteStructure(term, map);
    }

    public static Structure NegateTerm(this Structure term, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(functors);

        if
        (
            term.Functor != functors.NegationAsFailure
            ||
            term.Children.Count != 1
        )
        {
            return new Structure(functors.NegationAsFailure, [term]);
        }

        var innerStructMaybe = TermFuncs.ReturnStructureOrNone(term.Children[0]);

        if (!innerStructMaybe.HasValue)
        {
            return new Structure(functors.NegationAsFailure, [term]);
        }

        return innerStructMaybe.GetValueOrThrow();
    }

    public static IEnumerable<Variable> ExtractVariables(this ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Enumerate().OfType<Variable>().ToImmutableHashSet(TermFuncs.GetSingletonVariableComparer());
    }

    public static IEnumerable<ISimpleTerm> Enumerate(this ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return _flattener.Enumerate(term);
    }
}

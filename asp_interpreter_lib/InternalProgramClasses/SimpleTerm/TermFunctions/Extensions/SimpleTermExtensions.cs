using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using System.Collections.Immutable;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;

public static class SimpleTermExtensions
{
    private static SimpleTermFlattener _flattener = new ();

    private static SimpleTermEqualityComparer _equalityComparer = new ();

    private static SimpleTermContainsChecker _containsChecker = new ();

    private static SimpleTermHasher _hasher = new ();

    private static VariableSubstituter _variableSubstituter = new ();

    public static IEnumerable<ISimpleTerm> Enumerate(this ISimpleTerm simpleTerm)
    {
        return _flattener.ToList(simpleTerm);
    }

    public static bool IsEqualTo(this ISimpleTerm simpleTerm, ISimpleTerm other)
    {
        return _equalityComparer.Equals(simpleTerm, other);
    }

    public static bool Contains(this ISimpleTerm simpleTerm, ISimpleTerm other)
    {
        return _containsChecker.LeftContainsRight(simpleTerm, other);
    }

    public static int Hash(this ISimpleTerm simpleTerm)
    {
        return _hasher.Hash(simpleTerm);
    }

    public static IEnumerable<Variable> ExtractVariables(this ISimpleTerm term)
    {
        return term.Enumerate().OfType<Variable>().ToImmutableHashSet();
    }

    public static ISimpleTerm Substitute(this ISimpleTerm simpleTerm,Dictionary<Variable, ISimpleTerm> substitution)
    {
        return _variableSubstituter.Substitute(simpleTerm, substitution);
    }

    public static bool IsNegated(this ISimpleTerm term, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(functors);

        if 
        (
            term is Structure structure
            && structure.Functor == functors.NegationAsFailure
            && structure.Children.Count() == 1
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static ISimpleTerm NegateTerm(this ISimpleTerm term, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(functors);

        if 
        (
            term is Structure structure
            && structure.Functor == functors.NegationAsFailure
            && structure.Children.Count() == 1
        )
        {
            return structure.Children.ElementAt(0);
        }
        else
        {
            return new Structure(functors.NegationAsFailure, [term]);
        }
    }
}

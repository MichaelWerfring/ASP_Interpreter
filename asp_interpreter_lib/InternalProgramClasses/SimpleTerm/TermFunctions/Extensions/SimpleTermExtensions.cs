using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using System.Collections.Immutable;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;

public static class SimpleTermExtensions
{
    private static SimpleTermCloner _cloner = new SimpleTermCloner();
    private static SimpleTermFlattener _flattener = new SimpleTermFlattener();
    private static SimpleTermEqualityComparer _equalityComparer = new SimpleTermEqualityComparer();
    private static SimpleTermContainsChecker _containsChecker = new SimpleTermContainsChecker();
    private static SimpleTermHasher _hasher = new SimpleTermHasher();
    private static VariableExtractor _variableExtractor = new VariableExtractor();
    private static VariableSubstituter _variableSubstituter = new VariableSubstituter();

    public static IEnumerable<ISimpleTerm> ToList(this ISimpleTerm simpleTerm)
    {
        return _flattener.ToList(simpleTerm);
    }

    public static ISimpleTerm Clone(this ISimpleTerm simpleTerm)
    {
        return _cloner.Clone(simpleTerm);
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

    public static IImmutableSet<Variable> ExtractVariables(this ISimpleTerm simpleTerm)
    {
        return _variableExtractor.GetVariables(simpleTerm);
    }

    public static ISimpleTerm Substitute(this ISimpleTerm simpleTerm,Dictionary<Variable, ISimpleTerm> substitution)
    {
        return _variableSubstituter.Substitute(simpleTerm, substitution);
    }
}

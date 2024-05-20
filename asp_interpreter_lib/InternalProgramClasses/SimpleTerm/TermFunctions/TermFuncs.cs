using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public static class TermFuncs
{
    private static readonly ClauseVariableRenamer _renamer = new(new());

    private static readonly StructureReducer _reducer = new();

    private static readonly VariableComparer _varComparer = new();

    private static readonly SimpleTermComparer _termComparer = new();

    public static IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> Reduce(IStructure structure, IStructure other)
    {
        return _reducer.TryReduce(structure, other);
    }

    public static RenamingResult RenameClause(IEnumerable<ISimpleTerm> clause, int nextInternalIndex)
    {
        return _renamer.RenameVariables(clause, nextInternalIndex);
    }

    /// <summary>
    /// Gets a singleton variable comparer instance. 
    /// VariableComparer works by hashing so its faster than regular comparer.
    /// </summary>
    public static VariableComparer GetSingletonVariableComparer()
    {
        return _varComparer;
    }

    public static SimpleTermComparer GetSingletonTermComparer()
    {
        return _termComparer;
    }
}

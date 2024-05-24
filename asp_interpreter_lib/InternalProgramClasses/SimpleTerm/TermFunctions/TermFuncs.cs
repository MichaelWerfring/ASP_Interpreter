using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.Typechecking;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public static class TermFuncs
{
    private static readonly ClauseVariableRenamer _renamer = new(new());
    private static readonly StructureReducer _reducer = new();
    private static readonly VariableComparer _varComparer = new();
    private static readonly SimpleTermComparer _termComparer = new();
    private static readonly TermCaseDeterminer _caseDeterminer = new();
    private static readonly IntegerChecker _integerChecker = new();
    private static readonly VariableChecker _variableChecker = new();
    private static readonly StructureChecker _structureChecker = new();

    public static IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> Reduce(IStructure structure, IStructure other)
    {
        ArgumentNullException.ThrowIfNull(structure);
        ArgumentNullException.ThrowIfNull(other);

        return _reducer.TryReduce(structure, other);
    }

    public static RenamingResult RenameClause(IEnumerable<Structure> clause, int nextInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        return _renamer.RenameVariables(clause, nextInternalIndex);
    }

    public static IBinaryTermCase DetermineCase(ISimpleTerm left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return _caseDeterminer.DetermineCase(left, right);
    }

    public static IOption<Variable> ReturnVariableOrNone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return _variableChecker.ReturnVariableOrNone(term);
    }

    public static IOption<Integer> ReturnIntegerOrNone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return _integerChecker.ReturnIntegerOrNone(term);
    }

    public static IOption<Structure> ReturnStructureOrNone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return _structureChecker.ReturnStructureOrNone(term);
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

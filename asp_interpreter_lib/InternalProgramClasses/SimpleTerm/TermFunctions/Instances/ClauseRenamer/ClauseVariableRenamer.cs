using asp_interpreter_lib.FunctorNaming;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;

public class ClauseVariableRenamer
{
    private readonly VariableComparer _comparer;
    private readonly FunctorTableRecord _functors;

    public ClauseVariableRenamer(VariableComparer comparer, FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));

        _comparer = comparer;
        _functors = functors;
    }

    public RenamingResult RenameVariables(IEnumerable<ISimpleTerm> clause, int currentInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        var clauseVariables = clause.SelectMany(x => x.ExtractVariables()).Distinct(_comparer);

        var varsToNewVarsKeyValuePairs = new List<KeyValuePair<Variable, ISimpleTerm>>();
        foreach (var var in clauseVariables)
        {
            varsToNewVarsKeyValuePairs.Add(new KeyValuePair<Variable, ISimpleTerm>
                (var, new Variable($"{_functors.InternalVariable}{currentInternalIndex}")));

            currentInternalIndex += 1;
        }

        var dict = varsToNewVarsKeyValuePairs.ToDictionary(_comparer);

        var substitutedClause = clause.Select((term) => term.Substitute(dict));

        return new RenamingResult(substitutedClause, currentInternalIndex);
    }
}

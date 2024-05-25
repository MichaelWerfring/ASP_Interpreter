using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;

public class ClauseVariableRenamer
{
    private readonly FunctorTableRecord _functors;

    public ClauseVariableRenamer(FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));

        _functors = functors;
    }

    public RenamingResult RenameVariables(IEnumerable<Structure> clause, int currentInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        var clauseVariables = clause
            .SelectMany(x => x.ExtractVariables())
            .Distinct(TermFuncs.GetSingletonVariableComparer());

        var varsToNewVarsKeyValuePairs = new List<KeyValuePair<Variable, ISimpleTerm>>();
        foreach (var var in clauseVariables)
        {
            varsToNewVarsKeyValuePairs.Add(new KeyValuePair<Variable, ISimpleTerm>
                (var, new Variable($"{_functors.InternalVariable}{currentInternalIndex}")));

            currentInternalIndex += 1;
        }

        var dict = varsToNewVarsKeyValuePairs.
            ToDictionary(TermFuncs.GetSingletonVariableComparer());

        var substitutedClause = clause.Select((term) => term.Substitute(dict));

        return new RenamingResult(substitutedClause, currentInternalIndex);
    }
}

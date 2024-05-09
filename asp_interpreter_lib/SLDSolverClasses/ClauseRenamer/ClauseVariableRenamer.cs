using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.ClauseRenamer;

public class ClauseVariableRenamer
{
    private VariableComparer _comparer = new VariableComparer();

    public RenamingResult RenameVariables(IEnumerable<ISimpleTerm> clause, int currentInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        var clauseVariables = clause
            .SelectMany(x => x.Enumerate().OfType<Variable>())
            .ToImmutableHashSet(_comparer);

        var varsToNewVarsMapping = new Dictionary<Variable, ISimpleTerm>(_comparer);
        foreach (var variable in clauseVariables)
        {
            varsToNewVarsMapping.Add(variable, new Variable($"#VAR{currentInternalIndex}"));
            currentInternalIndex += 1;
        }

        return new RenamingResult(clause.Select((term) => term.Substitute(varsToNewVarsMapping)), currentInternalIndex);
    }

}

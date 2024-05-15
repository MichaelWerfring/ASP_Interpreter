using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;

namespace asp_interpreter_lib.SLDSolverClasses.ClauseRenamer;

public class ClauseVariableRenamer
{
    public RenamingResult RenameVariables(IEnumerable<ISimpleTerm> clause, int currentInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        var func = new FunctorTableRecord();
        var clauseVariables = clause.SelectMany(x => x.ExtractVariables()).Distinct(new VariableComparer());

        var varsToNewVarsKeyValuePairs = new List<KeyValuePair<Variable, ISimpleTerm>>();
        foreach( var var in clauseVariables )
        {
            varsToNewVarsKeyValuePairs.Add(new KeyValuePair<Variable, ISimpleTerm>(var, new Variable($"#Var{currentInternalIndex}")));
            currentInternalIndex += 1;
        }

        var substitutedClause = clause.Select((term) => term.Substitute(varsToNewVarsKeyValuePairs.ToDictionary(new VariableComparer())));

        return new RenamingResult(substitutedClause, currentInternalIndex);
    }
}

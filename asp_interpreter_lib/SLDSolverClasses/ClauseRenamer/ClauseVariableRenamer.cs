using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.SLDSolverClasses.VariableRenaming;

public class ClauseVariableRenamer
{
    private VariableExtractor _variableNameExtractor = new VariableExtractor();
    private VariableSubstituter _variableSubstituter = new VariableSubstituter();

    public RenamingResult RenameVariables(IEnumerable<ISimpleTerm> clause, int currentInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        var clauseVariables = clause
            .Select(_variableNameExtractor.GetVariableNames)
            .Aggregate((varSetA, varSetB) =>
            {
                return varSetA.Union(varSetB, new VariableComparer()).ToHashSet();
            });

        var varsToNewVarsMapping = new Dictionary<Variable, ISimpleTerm>(new VariableComparer());
        foreach (var variable in clauseVariables)
        {
            varsToNewVarsMapping.Add(variable, new Variable($"#VAR{currentInternalIndex}"));
            currentInternalIndex += 1;
        }

        return new RenamingResult(clause.Select((term) => _variableSubstituter.Substitute(term, varsToNewVarsMapping)), currentInternalIndex);
    }

}

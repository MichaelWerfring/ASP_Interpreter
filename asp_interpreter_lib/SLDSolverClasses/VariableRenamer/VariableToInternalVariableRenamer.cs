using asp_interpreter_lib.InternalProgramClasses.InternalTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;

namespace asp_interpreter_lib.SLDSolverClasses.VariableRenamer;

public class VariableToInternalVariableRenamer
{
    private VariableExtractor _variableNameExtractor = new VariableExtractor();
    private VariableSubstituter _variableSubstituter = new VariableSubstituter();

    public RenamingResult RenameVariables(IEnumerable<IInternalTerm> clause, int currentInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        var clauseVariables = clause
            .Select(_variableNameExtractor.GetVariableNames)
            .Aggregate((varSetA, varSetB) =>
            {
                return varSetA.Union(varSetB, new VariableComparer()).ToHashSet();
            });

        var varsToNewVarsMapping = new Dictionary<Variable, IInternalTerm>( new VariableComparer());
        foreach (var variable in clauseVariables)
        {
            varsToNewVarsMapping.Add(variable, new Variable($"#VAR{currentInternalIndex}"));
            currentInternalIndex += 1;
        }

        return new RenamingResult(clause.Select((term) => _variableSubstituter.Substitute(term, varsToNewVarsMapping)), currentInternalIndex);
    }

}

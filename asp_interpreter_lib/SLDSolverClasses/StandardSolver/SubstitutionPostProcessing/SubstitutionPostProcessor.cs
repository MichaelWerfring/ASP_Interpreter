using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.SLDSolverClasses.StandardSolver.VariableRenamer;

namespace asp_interpreter_lib.SLDSolverClasses.StandardSolver.SubstitutionPostProcessing;

internal class SubstitutionPostProcessor
{
    private TermFromMappingBuilder _mappingBuilder = new TermFromMappingBuilder();

    public Dictionary<Variable, ISimpleTerm> Simplify(Dictionary<Variable, ISimpleTerm> variableToTermMapping)
    {
        ArgumentNullException.ThrowIfNull(variableToTermMapping, nameof(variableToTermMapping));

        var queryVars = variableToTermMapping.Keys.Where((variable) => !variable.Identifier.StartsWith('#'));

        var mappingWithoutInternals = new Dictionary<Variable, ISimpleTerm>(new VariableComparer());
        foreach (var variable in queryVars)
        {
            var term = _mappingBuilder.BuildTerm(variable, variableToTermMapping);
            mappingWithoutInternals.Add(variable, term);
        }

        return mappingWithoutInternals;
    }
}

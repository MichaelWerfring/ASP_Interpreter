using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.PostProcessing;

internal class SubstitutionPostProcessor
{
    private TermFromMappingBuilder _mappingBuilder = new TermFromMappingBuilder();

    public Dictionary<Variable, ISimpleTerm> Simplify(Dictionary<Variable, ISimpleTerm> variableToTermMapping)
    {
        ArgumentNullException.ThrowIfNull(variableToTermMapping, nameof(variableToTermMapping));

        var queryVars = variableToTermMapping.Keys.Where((variable) => !variable.Identifier.StartsWith('#'));

        var mappingWithoutInternalsOnLeft = new Dictionary<Variable, ISimpleTerm>(new VariableComparer());
        foreach (var variable in queryVars)
        {
            var term = _mappingBuilder.BuildTerm(variable, variableToTermMapping);
            mappingWithoutInternalsOnLeft.Add(variable, term);
        }

        var mappingWithoutInternals = new Dictionary<Variable, ISimpleTerm>(new VariableComparer());
        foreach (var pair in mappingWithoutInternalsOnLeft)
        {
            if (pair.Value is Variable rightVar && rightVar.Identifier.StartsWith('#'))
            {
                continue;
            }

            mappingWithoutInternals.Add(pair.Key, pair.Value);
        }

        return mappingWithoutInternals;
    }
}

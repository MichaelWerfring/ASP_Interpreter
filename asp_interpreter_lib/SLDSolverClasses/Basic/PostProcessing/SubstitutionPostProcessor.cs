using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.PostProcessing;

internal class SubstitutionPostProcessor
{
    private readonly TermFromMappingBuilder _mappingBuilder = new();

    public Dictionary<Variable, ISimpleTerm> Simplify(Dictionary<Variable, ISimpleTerm> variableToTermMapping)
    {
        ArgumentNullException.ThrowIfNull(variableToTermMapping, nameof(variableToTermMapping));

        var queryVars = variableToTermMapping.Keys.Where((variable) => !variable.Identifier.StartsWith('#'));

        var mappingWithoutInternalsOnLeft = new Dictionary<Variable, ISimpleTerm>(TermFuncs.GetSingletonVariableComparer());
        foreach (var variable in queryVars)
        {
            var term = _mappingBuilder.BuildTerm(variable, variableToTermMapping);
            mappingWithoutInternalsOnLeft.Add(variable, term);
        }

        var mappingWithoutInternals = new Dictionary<Variable, ISimpleTerm>(TermFuncs.GetSingletonVariableComparer());
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

using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;

public class VariableMappingPostprocessor
{
    public VariableMapping Postprocess(VariableMapping mapping)
    {
        return Postprocess(mapping, mapping.Keys.Where(x => !x.Identifier.StartsWith('#')));
    }

    public VariableMapping Postprocess(VariableMapping variableMapping, IEnumerable<Variable> variablesToKeep)
    {
        ArgumentNullException.ThrowIfNull(variableMapping, nameof(variableMapping));
        ArgumentNullException.ThrowIfNull(variablesToKeep, nameof(variablesToKeep));

        // make a new binding, add noninternal variables and their simplified internalVariablesInTerms.
        var newBinding = new Dictionary<Variable, IVariableBinding>(new VariableComparer());
        foreach (var variable in variablesToKeep)
        {
            newBinding.Add(variable, variableMapping.Resolve(variable, true));
        }

        // get all variables from all the term bindings.
        var internalVariablesInTerms = newBinding.Values
            .OfType<TermBinding>()          
            .SelectMany(x => (x).Term.ExtractVariables())
            .Where(x => x.Identifier.StartsWith('#'))
            .ToImmutableHashSet(new VariableComparer());
 
        // for all the variables in the termbindings: add their values as well, if they have any.
        foreach (var variable in internalVariablesInTerms)
        {
            if (variableMapping.TryGetValue(variable, out IVariableBinding? bindings))
            {
                newBinding.Add(variable, bindings);
            }
        }

        return new VariableMapping(newBinding.ToImmutableDictionary(new VariableComparer()));
    }
}

using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;

public class VariableMappingPostprocessor
{
    private readonly BindingFromVariableMappingBuilder _builder = new BindingFromVariableMappingBuilder();

    public VariableMapping Postprocess(VariableMapping variableMapping)
    {
        ArgumentNullException.ThrowIfNull(variableMapping, nameof(variableMapping));

        // get noninternals
        var nonInternalVariables = variableMapping.Mapping.Keys
            .Where(x => !x.Identifier.StartsWith('#'));

        // make a new binding, add noninternal variables and their simplified unflattenedTerms.
        var newBinding = new Dictionary<Variable, IVariableBinding>(new VariableComparer());
        foreach ( var variable in nonInternalVariables )
        {
            newBinding.Add(variable, _builder.Build(variableMapping.Mapping[variable], variableMapping)) ;
        }

        // get all variables from all the termbindings.
        var unflattenedTerms = newBinding.Values
            .Where(x => x is TermBinding)
            .Select(x => ((TermBinding)x).Term)
            .Select(x => x.ToList());

        HashSet<Variable> variables = new HashSet<Variable> (new VariableComparer());
        foreach ( var terms in unflattenedTerms )
        {
            variables.Concat(terms.Where(x => x is Variable).Select(x => (Variable)x));
        }

        // for all the variables in the termbindings: add their values as well, if they have any.
        foreach (var variable in variables)
        {
            IVariableBinding? binding;
            variableMapping.Mapping.TryGetValue(variable, out binding);

            if(binding == null ) { continue; }

            newBinding.Add(variable, binding);
        }

        return new VariableMapping(newBinding.ToImmutableDictionary(new VariableComparer()));
    }
}

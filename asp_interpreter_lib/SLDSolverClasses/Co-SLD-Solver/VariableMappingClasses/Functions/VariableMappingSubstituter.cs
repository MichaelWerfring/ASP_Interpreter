using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;

public class VariableMappingSubstituter
{
    private BindingFromVariableMappingBuilder _builder = new BindingFromVariableMappingBuilder(true);

    public ISimpleTerm Substitute(ISimpleTerm term, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(mapping);

        var varsInTerm = term.Enumerate().OfType<Variable>().ToImmutableHashSet();

        var termBindingsOnly = mapping.Split().Item1
            .Select(x => (x.Key, x.Value.Term))
            .ToDictionary(new VariableComparer());
            
        return term.Substitute(termBindingsOnly);
    }
}

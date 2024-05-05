using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;

public class VariableMappingSubstituter
{
    public ISimpleTerm Substitute(ISimpleTerm term, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(mapping);

        var varToTermMapping = mapping.Mapping.Where(pair => pair.Value is TermBinding)
                                              .Select(pair => (pair.Key, ((TermBinding)pair.Value).Term))
                                              .ToDictionary(new VariableComparer());

        return term.Substitute(varToTermMapping);
    }
}

using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;

public class VariableMappingSubstituter
{
    private VariableSubstituter substituter = new VariableSubstituter();

    public ISimpleTerm Substitute(ISimpleTerm term, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(mapping);

        var varToTermMapping = mapping.Mapping.Where(pair => pair.Value is TermBinding)
                                              .Select(pair => (pair.Key, ((TermBinding)pair.Value).Term))
                                              .ToDictionary(new VariableComparer());

        return substituter.Substitute(term, varToTermMapping);
    }
}

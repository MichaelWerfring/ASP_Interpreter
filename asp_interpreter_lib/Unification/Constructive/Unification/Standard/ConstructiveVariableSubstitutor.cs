using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard;

public class ConstructiveVariableSubstitutor : IVariableBindingArgumentVisitor<ISimpleTerm, Variable>
{
    public ISimpleTerm GetSubstitutionOrDefault(Variable variable, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        IVariableBinding? value;
        mapping.Mapping.TryGetValue(variable, out value);

        if (value == null)
        {
            return variable;
        }

        return value.Accept(this, variable);
    }

    public ISimpleTerm Visit(ProhibitedValuesBinding binding, Variable args)
    {
        return args;
    }

    public ISimpleTerm Visit(TermBinding binding, Variable args)
    {
        return binding.Term;
    }
}

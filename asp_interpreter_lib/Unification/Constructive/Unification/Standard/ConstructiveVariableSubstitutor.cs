using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard;

public class ConstructiveVariableSubstitutor : IVariableBindingArgumentVisitor<ISimpleTerm, Variable>
{
    public ISimpleTerm TryGetSubstitution(ISimpleTerm term, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(term, nameof(term));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        if (term is not Variable variable)
        {
            return term;
        }
   
        if (!mapping.TryGetValue(variable, out IVariableBinding? value))
        {
            return term;
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

using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard;

internal class SubstitutionApplier : IVariableBindingArgumentVisitor<IVariableBinding, IDictionary<Variable, ISimpleTerm>>
{
    public VariableMapping ApplySubstitutionComposition(VariableMapping oldMapping, Variable var, ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(oldMapping);
        ArgumentNullException.ThrowIfNull(var);
        ArgumentNullException.ThrowIfNull(term);

        var dictForSubstitution = new Dictionary<Variable, ISimpleTerm>(TermFuncs.GetSingletonVariableComparer())
        {
            { var, term }
        };

        var newMap = oldMapping;

        foreach (var pair in oldMapping)
        {
            newMap = newMap.SetItem(pair.Key, pair.Value.Accept(this, dictForSubstitution));
        }

        return newMap.SetItem(var, new TermBinding(term));
    }

    public IVariableBinding Visit(ProhibitedValuesBinding binding, IDictionary<Variable, ISimpleTerm> substitution)
    {
        ArgumentNullException.ThrowIfNull(binding);
        ArgumentNullException.ThrowIfNull(substitution);

        return binding;
    }

    public IVariableBinding Visit(TermBinding binding, IDictionary<Variable, ISimpleTerm> substitution)
    {
        ArgumentNullException.ThrowIfNull(binding);
        ArgumentNullException.ThrowIfNull(substitution);

        return new TermBinding(binding.Term.Substitute(substitution));
    }
}

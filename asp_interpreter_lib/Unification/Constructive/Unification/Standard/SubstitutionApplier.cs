using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard
{
    internal class SubstitutionApplier
    {
        public VariableMapping ApplySubstitutionComposition(VariableMapping oldMapping, Variable var, ISimpleTerm term)
        {
            var dictForSubstitution = new Dictionary<Variable, ISimpleTerm>(TermFuncs.GetSingletonVariableComparer())
            {
                { var, term }
            };

            VariableMapping newMap = oldMapping;

            foreach (var pair in oldMapping)
            {
                if (pair.Value is TermBinding binding)
                {
                    newMap = newMap.SetItem(pair.Key, new TermBinding(binding.Term.Substitute(dictForSubstitution)));
                }
            }

            return newMap.SetItem(var, new TermBinding(term));
        }
    }
}

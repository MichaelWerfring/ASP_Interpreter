using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;

public class BindingFromVariableMappingBuilder : IVariableBindingArgumentVisitor<IVariableBinding, VariableMapping>
{
    public IVariableBinding Build(IVariableBinding binding, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        return binding.Accept(this, mapping);
    }

    public IVariableBinding Visit(ProhibitedValuesBinding binding, VariableMapping args)
    {
        return binding;
    }

    public IVariableBinding Visit(TermBinding binding, VariableMapping args)
    {
       if (binding.Term is Variable var)
       {
            if (args.Mapping.TryGetValue(var, out IVariableBinding? value))
            {
                return value.Accept(this, args);
            }
            else
            {
                return binding;
            }           
       }

       // wrap the variables in termbindings so we can visit them recursively.
        var variablesInTermAsTermbindings = binding.Term.ToList()
                                                .Where(x => x is Variable)
                                                .ToHashSet(new SimpleTermEqualityComparer())
                                                .Select(x => new TermBinding(x));

        // resolve those variables : get only the termbindings.
        var resolvedVars = variablesInTermAsTermbindings
            .Select(x => ((Variable)x.Term, x.Accept(this, args)))
            .Where(pair => pair.Item2 is TermBinding)
            .Select(pair => (pair.Item1, ((TermBinding)pair.Item2).Term))
            .ToDictionary(new VariableComparer());

        return new TermBinding(binding.Term.Substitute(resolvedVars));
    }
}

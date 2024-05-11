using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;

public class BindingSimplifier : IVariableBindingArgumentVisitor<IVariableBinding, VariableMapping>,
                                 ISimpleTermArgsVisitor<IVariableBinding, VariableMapping>
{
    private bool _doProhibitedValuesBindingResolution;

    public BindingSimplifier(bool doProhibitedValuesBindingResolution)
    {
        _doProhibitedValuesBindingResolution = doProhibitedValuesBindingResolution;
    }

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

        return binding.Term.Accept(this, args);      
    }

    public IVariableBinding Visit(Variable variableTerm, VariableMapping map)
    {
        IVariableBinding? binding;
        if (!map.Mapping.TryGetValue(variableTerm, out binding))
        {
            return new TermBinding(variableTerm);
        }

        if (!_doProhibitedValuesBindingResolution && binding is ProhibitedValuesBinding)
        {
            return new TermBinding(variableTerm);
        }

        return binding.Accept(this, map);
    }

    public IVariableBinding Visit(Structure basicTerm, VariableMapping map)
    {
        // get variables in term
        var variablesInTerm = basicTerm.Enumerate()
            .OfType<Variable>()
            .ToImmutableHashSet(new VariableComparer());

        // filter out all variables where you have something like this : X => s(X).
        // Var must:
        // Map to something
        // && map to a termbinding that is not the input termbinding.
        var filteredVariables = variablesInTerm.Where
        (
            x =>
            map.Mapping.TryGetValue(x, out IVariableBinding? varBinding)
            &&
            (varBinding is TermBinding tb && !tb.Term.IsEqualTo(basicTerm))
        );

        // resolve those variables : get only the termbindings.
        var resolvedVars = filteredVariables
            .Select(x => (x, Visit(new TermBinding(x), map)))
            .Where(pair => pair.Item2 is TermBinding)
            .Select(pair => (pair.Item1, ((TermBinding)pair.Item2).Term))
            .ToDictionary(new VariableComparer());

        return new TermBinding(basicTerm.Substitute(resolvedVars));
    }

    public IVariableBinding Visit(Integer integer, VariableMapping arguments)
    {
        return new TermBinding(integer);
    }
}

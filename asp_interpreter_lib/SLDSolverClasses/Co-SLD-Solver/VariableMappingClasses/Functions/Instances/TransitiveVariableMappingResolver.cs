using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;

public class TransitiveVariableMappingResolver : IVariableBindingArgumentVisitor<IVariableBinding, VariableMapping>,
                                                 ISimpleTermArgsVisitor<IVariableBinding, VariableMapping>
{
    /// <summary>
    /// Whether X -> Y -> \={1,2,3}
    /// should resolve to X -> \={1,2,3},
    /// or just X -> Y.
    /// </summary>
    private readonly bool _doProhibitedValuesBindingResolution;

    public TransitiveVariableMappingResolver(bool doProhibitedValuesBindingResolution)
    {
        _doProhibitedValuesBindingResolution = doProhibitedValuesBindingResolution;
    }

    /// <summary>
    /// Transitively simplifies a variableBinding, ie. if X => Y => s(), then X => s().
    /// Handles self-recursive structures like so: X => s(X) just returns s(X). However: X => s(X, Y), Y => 1 would resolve to s(X, 1).
    /// </summary>
    public IOption<IVariableBinding> Resolve(Variable variable, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        if (!mapping.TryGetValue(variable, out IVariableBinding? value))
        {
            return new None<IVariableBinding>();
        }

        return new Some<IVariableBinding>(value.Accept(this, mapping));
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
        if (!map.TryGetValue(variableTerm, out IVariableBinding? binding))
        {
            return new TermBinding(variableTerm);
        }

        if (!_doProhibitedValuesBindingResolution && binding is ProhibitedValuesBinding)
        {
            return new TermBinding(variableTerm);
        }

        return binding.Accept(this, map);
    }

    public IVariableBinding Visit(Structure structure, VariableMapping map)
    {
        // get variables in term
        var variablesInTerm = structure.ExtractVariables();

        // filter out all variables where you have something like this : X => s(X).
        // Var must:
        // Map to something
        // and map to a termbinding that is not the input termbinding.
        var filteredVariables = variablesInTerm.Where
        (
            x =>
            map.TryGetValue(x, out IVariableBinding? varBinding)
            &&
            (varBinding is TermBinding tb && !tb.Term.IsEqualTo(structure))
        );

        // resolve those variables : get only the termbindings. Build mapping.
        var resolvedVars = filteredVariables
            .Select(x => (x, Visit(new TermBinding(x), map)))
            .Where(pair => pair.Item2 is TermBinding)
            .Select(pair => (pair.x, ((TermBinding)pair.Item2).Term))
            .ToDictionary(TermFuncs.GetSingletonVariableComparer());

        // substitute using that dictionary.
        var substitutedStruct = structure.Substitute(resolvedVars);

        return new TermBinding(substitutedStruct);
    }

    public IVariableBinding Visit(Integer integer, VariableMapping arguments)
    {
        return new TermBinding(integer);
    }
}
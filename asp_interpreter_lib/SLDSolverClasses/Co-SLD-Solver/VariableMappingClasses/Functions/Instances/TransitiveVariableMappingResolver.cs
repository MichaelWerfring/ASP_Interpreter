using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.Splitter;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;

public class TransitiveVariableMappingResolver : IVariableBindingArgumentVisitor<IVariableBinding, VariableMapping>,
                                                 ISimpleTermArgsVisitor<IVariableBinding, VariableMapping>
{
    /// <summary>
    /// Whether X -> Y -> \={1,2,3}
    /// should resolve to X -> \={1,2,3},
    /// or just X -> Y.
    /// </summary>
    private readonly bool _doProhibitedValuesBindingResolution;

    private readonly TermBindingChecker _termbindingFilterer;

    public TransitiveVariableMappingResolver(bool doProhibitedValuesBindingResolution)
    {
        _doProhibitedValuesBindingResolution = doProhibitedValuesBindingResolution;
        _termbindingFilterer = new();
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

    public IVariableBinding Visit(ProhibitedValuesBinding binding, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(binding);
        ArgumentNullException.ThrowIfNull(map);

        return binding;
    }

    public IVariableBinding Visit(TermBinding binding, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(binding);
        ArgumentNullException.ThrowIfNull(map);

        return binding.Term.Accept(this, map);      
    }

    public IVariableBinding Visit(Variable term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        if (!map.TryGetValue(term, out IVariableBinding? binding))
        {
            return new TermBinding(term);
        }

        if (!_doProhibitedValuesBindingResolution && VarMappingFunctions.ReturnProhibitedValueBindingOrNone(binding).HasValue)
        {
            return new TermBinding(term);
        }

        return binding.Accept(this, map);
    }

    public IVariableBinding Visit(Structure term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        // get variables in term
        var variablesInTerm = term.ExtractVariables();

        // filter out all variables where you have something like this : X => s(X).
        var filteredVariables = variablesInTerm.Where
        (
            x =>
            {
                if (!map.TryGetValue(x, out IVariableBinding? value))
                {
                    return false;
                }

                var tbMaybe = VarMappingFunctions.ReturnTermbindingOrNone(value);

                if (!tbMaybe.HasValue) 
                {
                    return false; 
                }

                if (tbMaybe.GetValueOrThrow().Term.IsEqualTo(term))
                {
                    return false;
                }

                return true;
            }

        );

        // resolve those variables : get only the termbindings. Build mapping.
        var resolvedVars = filteredVariables
            .Select(x => (x, Visit(new TermBinding(x), map)))
            .Select(pair => (pair.x, _termbindingFilterer.ReturnTermbindingOrNone(pair.Item2)))
            .Where(pair => pair.Item2.HasValue)
            .Select(pair => (pair.x, pair.Item2.GetValueOrThrow().Term))
            .ToDictionary(TermFuncs.GetSingletonVariableComparer());

        // substitute using that dictionary.
        var substitutedStruct = term.Substitute(resolvedVars);

        return new TermBinding(substitutedStruct);
    }

    public IVariableBinding Visit(Integer term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return new TermBinding(term);
    }
}
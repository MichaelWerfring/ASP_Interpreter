﻿// <copyright file="TransitiveVariableMappingResolver.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.Splitter;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for transitively resolving where a <see cref="Variable"/> maps to in a <see cref="VariableMapping"/>.
/// </summary>
public class TransitiveVariableMappingResolver : IVariableBindingArgumentVisitor<IVariableBinding, VariableMapping>,
                                                 ISimpleTermArgsVisitor<IVariableBinding, VariableMapping>
{
    private readonly bool doProhibitedValuesBindingResolution;

    private readonly TermBindingChecker termbindingFilterer;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransitiveVariableMappingResolver"/> class.
    /// </summary>
    /// <param name="doProhibitedValuesBindingResolution">Whether X -> Y -> \={1,2,3}
    /// should resolve to X -> \={1,2,3},
    /// or just X -> Y.</param>
    public TransitiveVariableMappingResolver(bool doProhibitedValuesBindingResolution)
    {
        this.doProhibitedValuesBindingResolution = doProhibitedValuesBindingResolution;
        this.termbindingFilterer = new();
    }

    /// <summary>
    /// Transitively simplifies a variableBinding, ie. if X => Y => s(A, Z), and A -> a, B -> b, then X => s(a, b).
    /// Handles self-recursive structures like so: X => s(X) just returns s(X). However: X => s(X, Y), Y => 1 would resolve to s(X, 1).
    /// </summary>
    /// <param name="variable">The variable to resolve.</param>
    /// <param name="mapping">The mapping.</param>
    /// <returns>A resolved <see cref="IVariableBinding"/>, or none if variable is not in mapping.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="variable"/> is null,
    /// ..<paramref name="mapping"/> is null.</exception>
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

    /// <summary>
    /// Visits a binding and proceeds according to type.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    /// <param name="map">The mapping as an additional argument.</param>
    /// <returns>The resolved binding, in this case just the input binding.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="binding"/> is null,
    /// ..<paramref name="map"/> is null.</exception>
    public IVariableBinding Visit(ProhibitedValuesBinding binding, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(binding);
        ArgumentNullException.ThrowIfNull(map);

        return binding;
    }

    /// <summary>
    /// Visits a binding and proceeds according to type.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    /// <param name="map">The mapping as an additional argument.</param>
    /// <returns>The resolved binding, in this case the recursively resolved term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="binding"/> is null,
    /// ..<paramref name="map"/> is null.</exception>
    public IVariableBinding Visit(TermBinding binding, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(binding);
        ArgumentNullException.ThrowIfNull(map);

        return binding.Term.Accept(this, map);
    }

    /// <summary>
    /// Visits a variable term.
    /// </summary>
    /// <param name="term">The term to visit.</param>
    /// <param name="map">The map as an additional argument.</param>
    /// <returns>The resolved binding. If variable does not map to a value, then the input variable.
    /// If variable maps to a prohibited value binding, then the prohibited value or the variable, depending on <see cref="doProhibitedValuesBindingResolution"/>
    /// If variable maps to a term, then recursively resolve the term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="term"/> is null,
    /// ..<paramref name="map"/> is null.</exception>
    public IVariableBinding Visit(Variable term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        if (!map.TryGetValue(term, out IVariableBinding? binding))
        {
            return new TermBinding(term);
        }

        if (!this.doProhibitedValuesBindingResolution && VarMappingFunctions.ReturnProhibitedValueBindingOrNone(binding).HasValue)
        {
            return new TermBinding(term);
        }

        return binding.Accept(this, map);
    }

    /// <summary>
    /// Visits a structure term.
    /// </summary>
    /// <param name="term">The term to visit.</param>
    /// <param name="map">The map as an additional argument.</param>
    /// <returns> The resolved binding. Recursively resolves all the variables in the term.
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="term"/> is null,
    /// ..<paramref name="map"/> is null.</exception>
    public IVariableBinding Visit(Structure term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        // get variables in term
        var variablesInTerm = term.ExtractVariables();

        // filter out all variables where you have something like this : X => s(X).
        var filteredVariables = variablesInTerm.Where(
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
            });

        // resolve those variables : get only the termbindings. Build mapping.
        var resolvedVars = filteredVariables
            .Select(x => (x, this.Visit(new TermBinding(x), map)))
            .Select(pair => (pair.x, this.termbindingFilterer.ReturnTermbindingOrNone(pair.Item2)))
            .Where(pair => pair.Item2.HasValue)
            .Select(pair => (pair.x, pair.Item2.GetValueOrThrow().Term))
            .ToDictionary(TermFuncs.GetSingletonVariableComparer());

        // substitute using that dictionary.
        var substitutedStruct = term.Substitute(resolvedVars);

        return new TermBinding(substitutedStruct);
    }

    /// <summary>
    /// Visits an integer term.
    /// </summary>
    /// <param name="term">The term to visit.</param>
    /// <param name="map">The map as an additional argument.</param>
    /// <returns>The resolved binding. In this case, the integer wrapped in a termBinding.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="term"/> is null,
    /// ..<paramref name="map"/> is null.</exception>
    public IVariableBinding Visit(Integer term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return new TermBinding(term);
    }
}
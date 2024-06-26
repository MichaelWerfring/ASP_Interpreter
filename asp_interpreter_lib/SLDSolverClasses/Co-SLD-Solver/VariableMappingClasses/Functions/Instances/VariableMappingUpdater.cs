﻿// <copyright file="VariableMappingUpdater.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

/// <summary>
/// A class for updating.
/// </summary>
internal class VariableMappingUpdater : IBinaryVariableBindingCaseVisitor<IOption<IVariableBinding>>
{
    /// <summary>
    /// Updates <see cref="VariableMapping"/> left by values in <see cref="VariableMapping"/> right like so:
    /// For every variable key in the union of left and right:
    /// if only left has value, take left.
    /// if only right has value, take right.
    /// if left and right has value:
    /// if left is prohib and right is prohib, take their union.
    /// if left is prohib and right is term, take right.
    /// if left is term and right is prohib, fail.
    /// if left is term and right is term, fail if they are different or just take right.
    /// </summary>
    /// <param name="left">The mapping to update.</param>
    /// <param name="right">The mapping to update with.</param>
    /// <returns>The updated mapping, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="left"/> is null,
    /// ..<paramref name="right"/> is null.</exception>
    public IOption<VariableMapping> Update(VariableMapping left, VariableMapping right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        var variables = left.Keys.Union(right.Keys, TermFuncs.GetSingletonVariableComparer());

        var newPairs = new KeyValuePair<Variable, IVariableBinding>[variables.Count()];

        bool clashEncountered = false;
        Parallel.For(0, newPairs.Length, index =>
        {
            var currentVariable = variables.ElementAt(index);

            IOption<IVariableBinding> resolutionMaybe = this.Resolve(currentVariable, left, right);
            if (!resolutionMaybe.HasValue)
            {
                clashEncountered = true;
            }
            else
            {
                newPairs[index] = new KeyValuePair<Variable, IVariableBinding>(currentVariable, resolutionMaybe.GetValueOrThrow());
            }
        });

        if (clashEncountered)
        {
            return new None<VariableMapping>();
        }

        var newValues = ImmutableDictionary.CreateRange(TermFuncs.GetSingletonVariableComparer(), newPairs);

        return new Some<VariableMapping>(new VariableMapping(newValues));
    }

    /// <summary>
    /// Updates a case where both are <see cref="ProhibitedValuesBinding"/>.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>The union of both their prohibited value lists.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public IOption<IVariableBinding> Visit(ProhibValsProhibValsCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase, nameof(binaryCase));

        return new Some<IVariableBinding>(new ProhibitedValuesBinding(binaryCase.Left.ProhibitedValues.Union(binaryCase.Right.ProhibitedValues)));
    }

    /// <summary>
    /// Updates a case where left is <see cref="ProhibitedValuesBinding"/> and right is <see cref="TermBinding"/>.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>The right term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public IOption<IVariableBinding> Visit(ProhibValsTermBindingCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase, nameof(binaryCase));

        return new Some<IVariableBinding>(binaryCase.Right);
    }

    /// <summary>
    /// Updates a case where left is <see cref="TermBinding"/> and right is <see cref="ProhibitedValuesBinding"/>.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>Always none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public IOption<IVariableBinding> Visit(TermBindingProhibValsCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase, nameof(binaryCase));

        return new None<IVariableBinding>();
    }

    /// <summary>
    /// Updates a case where left is <see cref="TermBinding"/> and right is <see cref="TermBinding"/>.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>The right one, or none in case they are different.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public IOption<IVariableBinding> Visit(TermBindingTermBindingCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase, nameof(binaryCase));

        if (!binaryCase.Left.Term.IsEqualTo(binaryCase.Right.Term))
        {
            return new None<IVariableBinding>();
        }

        return new Some<IVariableBinding>(binaryCase.Right);
    }

    private IOption<IVariableBinding> Resolve(Variable variable, VariableMapping left, VariableMapping right)
    {
        if (!left.TryGetValue(variable, out IVariableBinding? leftVal))
        {
            return new Some<IVariableBinding>(right[variable]);
        }

        if (!right.TryGetValue(variable, out IVariableBinding? rightVal))
        {
            return new Some<IVariableBinding>(left[variable]);
        }

        var typeCase = VarMappingFunctions.DetermineCase(leftVal, rightVal);

        return typeCase.Accept(this);
    }
}
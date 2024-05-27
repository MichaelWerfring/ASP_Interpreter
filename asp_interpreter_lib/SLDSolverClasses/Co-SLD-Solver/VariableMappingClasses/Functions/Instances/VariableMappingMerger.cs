// <copyright file="VariableMappingMerger.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

/// <summary>
/// A class for merging two dictionaries containing <see cref="Variable"/> to <see cref="ProhibitedValuesBinding"/> mappings
/// and <see cref="Variable"/> to <see cref="TermBinding"/> mappings into a <see cref="VariableMapping"/>.
/// </summary>
internal class VariableMappingMerger
{
    /// <summary>
    /// Merges two dictionaries containing <see cref="Variable"/> to <see cref="ProhibitedValuesBinding"/> mappings
    /// and <see cref="Variable"/> to <see cref="TermBinding"/> mappings into a <see cref="VariableMapping"/>.
    /// In case of clashes, termbindings are preferred.
    /// </summary>
    /// <param name="prohibs">The first dictionary.</param>
    /// <param name="termbindings">The second dictionary.</param>
    /// <returns>A variableMapping containg the merging of both inputs.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="prohibs"/> is null,
    /// ..<paramref name="termbindings"/> is null.</exception>
    public VariableMapping Merge(
        IDictionary<Variable, ProhibitedValuesBinding> prohibs,
        IDictionary<Variable, TermBinding> termbindings)
    {
        ArgumentNullException.ThrowIfNull(prohibs, nameof(prohibs));
        ArgumentNullException.ThrowIfNull(termbindings, nameof(termbindings));

        var variables = prohibs.Keys.Union(termbindings.Keys, TermFuncs.GetSingletonVariableComparer());

        var dict = ImmutableDictionary.Create<Variable, IVariableBinding>(TermFuncs.GetSingletonVariableComparer());
        foreach (var variable in variables)
        {
            dict = dict.SetItem(variable, this.GetAppropriateBinding(variable, prohibs, termbindings));
        }

        return new VariableMapping(dict);
    }

    private IVariableBinding GetAppropriateBinding(
        Variable variable,
        IDictionary<Variable, ProhibitedValuesBinding> prohibs,
        IDictionary<Variable, TermBinding> termbindings)
    {
        prohibs.TryGetValue(variable, out ProhibitedValuesBinding? prohibsValue);
        termbindings.TryGetValue(variable, out TermBinding? termBindingValue);

        if (prohibsValue != null && termBindingValue != null)
        {
            return termBindingValue;
        }

        if (prohibsValue != null)
        {
            return prohibsValue;
        }

        return termBindingValue!;
    }
}
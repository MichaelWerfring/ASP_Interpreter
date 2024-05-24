﻿using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;

internal class VariableMappingMerger
{
    public VariableMapping Merge
    (
        IDictionary<Variable, ProhibitedValuesBinding> prohibs,
        IDictionary<Variable, TermBinding> termbindings
    )
    {
        ArgumentNullException.ThrowIfNull(prohibs, nameof(prohibs));
        ArgumentNullException.ThrowIfNull(termbindings, nameof(termbindings));

        var variables = prohibs.Keys.Union(termbindings.Keys, TermFuncs.GetSingletonVariableComparer());

        var dict = ImmutableDictionary.Create<Variable, IVariableBinding>(TermFuncs.GetSingletonVariableComparer());
        foreach (var variable in variables)
        {
            dict = dict.SetItem(variable, GetAppropriateBinding(variable, prohibs, termbindings));
        }

        return new VariableMapping(dict);
    }

    private IVariableBinding GetAppropriateBinding
    (
        Variable variable,
        IDictionary<Variable, ProhibitedValuesBinding> prohibs,
        IDictionary<Variable, TermBinding> termbindings
    )
    {
        prohibs.TryGetValue(variable, out ProhibitedValuesBinding? prohibsValue);
        termbindings.TryGetValue(variable, out TermBinding? termBindingValue);

        if (prohibsValue is not null && termBindingValue is not null)
        {
            return termBindingValue;
        }

        if (prohibsValue is not null)
        {
            return prohibsValue;
        }

        return termBindingValue!;
    }
}

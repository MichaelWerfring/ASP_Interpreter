﻿using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using System.Collections.Immutable;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard;

internal class ProhibitedValuesUpdater
{
    // updating
    public VariableMapping UpdateProhibitedValues(Variable left, Variable right, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(mapping);

        ProhibitedValuesBinding leftVals = 
            (ProhibitedValuesBinding)mapping[left] 
            ??
            throw new ArgumentException($"Must map to an instance of {nameof(ProhibitedValuesBinding)}", nameof(left));

        ProhibitedValuesBinding rightVals = (ProhibitedValuesBinding)mapping[right]
            ??
            throw new ArgumentException($"Must map to an instance of {nameof(ProhibitedValuesBinding)}", nameof(right));


        ImmutableSortedSet<ISimpleTerm> union = leftVals.ProhibitedValues
                                                    .Union(rightVals.ProhibitedValues);

       return mapping.SetItem
        (
            right,
            new ProhibitedValuesBinding(union)
        );
    }
}
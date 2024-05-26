// <copyright file="ProhibitedValuesChecker.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.Splitter;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Util.ErrorHandling;

internal class ProhibitedValuesChecker : IVariableBindingVisitor<IOption<ProhibitedValuesBinding>>
{
    public IOption<ProhibitedValuesBinding> ReturnProhibitedValueBindingOrNone(IVariableBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return binding.Accept(this);
    }

    public IOption<ProhibitedValuesBinding> Visit(ProhibitedValuesBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return new Some<ProhibitedValuesBinding>(binding);
    }

    public IOption<ProhibitedValuesBinding> Visit(TermBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return new None<ProhibitedValuesBinding>();
    }
}

// <copyright file="ProhibitedValuesChecker.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.Splitter;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for checking if a <see cref="IVariableBinding"/> is of type <see cref="ProhibitedValuesBinding"/>.
/// </summary>
internal class ProhibitedValuesChecker : IVariableBindingVisitor<IOption<ProhibitedValuesBinding>>
{
    /// <summary>
    /// Checks if a <see cref="IVariableBinding"/> is of type <see cref="ProhibitedValuesBinding"/>.
    /// </summary>
    /// <param name="binding">The binding to check.</param>
    /// <returns>The binding as a <see cref="ProhibitedValuesBinding"/>, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binding"/> is null.</exception>
    public IOption<ProhibitedValuesBinding> ReturnProhibitedValueBindingOrNone(IVariableBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return binding.Accept(this);
    }

    /// <summary>
    /// Visits a binding to check if it is of type <see cref="ProhibitedValuesBinding"/>.
    /// </summary>
    /// <param name="binding">The binding to check.</param>
    /// <returns>The binding as a <see cref="ProhibitedValuesBinding"/>, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binding"/> is null.</exception>
    public IOption<ProhibitedValuesBinding> Visit(ProhibitedValuesBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return new Some<ProhibitedValuesBinding>(binding);
    }

    /// <summary>
    /// Visits a binding to check if it is of type <see cref="ProhibitedValuesBinding"/>.
    /// </summary>
    /// <param name="binding">The binding to check.</param>
    /// <returns>The binding as a <see cref="ProhibitedValuesBinding"/>, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binding"/> is null.</exception>
    public IOption<ProhibitedValuesBinding> Visit(TermBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding, nameof(binding));

        return new None<ProhibitedValuesBinding>();
    }
}
// <copyright file="ValueRetriever.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Target.Builder;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Util.ErrorHandling.Either;

/// <summary>
/// A class for retrieving the appropriate binding from a mapping for a variable, or an error in case of the binding being a termbinding.
/// This has to be done with an instance-based class since static classed cant be visitors.
/// </summary>
internal class ValueRetriever : IVariableBindingVisitor<IEither<TargetBuildingException, ProhibitedValuesBinding>>
{
    /// <summary>
    /// Gets the variable's prohibited values binding, or a new one if it doesnt have one, or an error if it maps to a term.
    /// </summary>
    /// <param name="variable">The variable to retrieve the value for.</param>
    /// <param name="mapping">The mapping.</param>
    /// <returns>Either an exception or a prohibted values binding. Either a new one or the one the variable maps to.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..variable is null.
    /// ..mapping is null.</exception>
    public IEither<TargetBuildingException, ProhibitedValuesBinding> GetProhibitedValuesOrError(Variable variable, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        if (!mapping.TryGetValue(variable, out IVariableBinding? value))
        {
            return new Right<TargetBuildingException, ProhibitedValuesBinding>(new ProhibitedValuesBinding());
        }

        return value.Accept(this);
    }

    /// <summary>
    /// Visits a binding.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    /// <returns>In this case, the binding wrapped in an either.</returns>
    public IEither<TargetBuildingException, ProhibitedValuesBinding> Visit(ProhibitedValuesBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding);

        return new Right<TargetBuildingException, ProhibitedValuesBinding>(binding);
    }

    /// <summary>
    /// Visits a binding.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    /// <returns>In this case, an error since the variable mapped to a term.</returns>
    public IEither<TargetBuildingException, ProhibitedValuesBinding> Visit(TermBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding);

        return new Left<TargetBuildingException, ProhibitedValuesBinding>(
            new TargetBuildingException($"Failed to build target: mapping contained termbinding."));
    }
}
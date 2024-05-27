// <copyright file="StandardConstructiveDisunificationAlgorithm.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Disunification.Standard;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using Asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Util.ErrorHandling.Either;

/// <summary>
/// A class that acts as a simple disunification algorithm.
/// </summary>
public class StandardConstructiveDisunificationAlgorithm : IConstructiveDisunificationAlgorithm
{
    private readonly bool doGroundednessCheck;
    private readonly bool doDisunifyUnboundVariables;

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardConstructiveDisunificationAlgorithm"/> class.
    /// </summary>
    /// <param name="doGroundednessCheck">Whether to check for groundedness of terms.</param>
    /// <param name="doDisunifyVariables">Whether to disunify two variables with each other.</param>
    public StandardConstructiveDisunificationAlgorithm(bool doGroundednessCheck, bool doDisunifyVariables)
    {
        this.doGroundednessCheck = doGroundednessCheck;
        this.doDisunifyUnboundVariables = doDisunifyVariables;
    }

    /// <summary>
    /// Attempts to disunify a constructive target.
    /// </summary>
    /// <param name="target">The target to disunify.</param>
    /// <returns>Either a disunification exception or an enumerable of result mappings.</returns>
    /// <exception cref="ArgumentNullException">Thrown if target is null.</exception>
    public IEither<DisunificationException, IEnumerable<VariableMapping>> Disunify(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        // create new disunifier instance
        var constructiveDisunifier = new ConstructiveDisunifier(
            this.doGroundednessCheck,
            this.doDisunifyUnboundVariables,
            target);

        // if error during disunification, return error.
        var disunifiersEither = constructiveDisunifier.Disunify();
        IEnumerable<DisunificationResult> disunifiers;
        try
        {
            disunifiers = disunifiersEither.GetRightOrThrow();
        }
        catch
        {
            return new Left<DisunificationException, IEnumerable<VariableMapping>>(
                disunifiersEither.GetLeftOrThrow());
        }

        // wrap input mapping into a more generic variable mapping
        var inputMappingAsVariableMapping = new VariableMapping(target.Mapping);

        // if disunifiers is empty(target disunifies anyways), just return the mapping
        if (!disunifiers.Any())
        {
            return new Right<DisunificationException, IEnumerable<VariableMapping>>([inputMappingAsVariableMapping]);
        }

        // create a new mapping for every disunifier where the value is updated by the disunifier value.
        var newMappings = disunifiers.Select(disunifier =>
        {
            if (disunifier.IsInstantiation)
            {
                return inputMappingAsVariableMapping.SetItem(disunifier.Variable, new TermBinding(disunifier.Term));
            }
            else
            {
                var prohibitedValueList = target.Mapping[disunifier.Variable];

                return inputMappingAsVariableMapping.SetItem(
                    disunifier.Variable,
                    new ProhibitedValuesBinding(prohibitedValueList.ProhibitedValues.Add(disunifier.Term)));
            }
        });

        return new Right<DisunificationException, IEnumerable<VariableMapping>>(newMappings);
    }
}
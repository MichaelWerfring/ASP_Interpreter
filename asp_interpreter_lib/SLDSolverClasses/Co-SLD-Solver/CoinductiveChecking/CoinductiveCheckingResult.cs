// <copyright file="CoinductiveCheckingResult.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

/// <summary>
/// A class that represents a successful coinductive checking result.
/// </summary>
public class CoinductiveCheckingResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CoinductiveCheckingResult"/> class.
    /// </summary>
    /// <param name="target">The checking target.</param>
    /// <param name="map">The result mapping. This might be different from the checking input mapping, depending on success type.</param>
    /// <param name="successType">The success type.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="target"/> is null,
    /// <paramref name="map"/> is null.</exception>
    public CoinductiveCheckingResult(Structure target, VariableMapping map, SuccessType successType)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(map, nameof(map));

        this.Target = target;
        this.Mapping = map;
        this.SuccessType = successType;
    }

    /// <summary>
    /// Gets the checking target. This might be different from the checking input target,
    /// since a constraintment result might have replaced variables by terms.
    /// </summary>
    public Structure Target { get; }

    /// <summary>
    /// Gets the mapping. This might be different from the checking input mapping, depending on success type.
    /// </summary>
    public VariableMapping Mapping { get; }

    /// <summary>
    /// Gets the success type.
    /// </summary>
    public SuccessType SuccessType { get; }
}

public enum SuccessType
{
    DeterministicSuccess, NonDeterministicSuccess, NoMatch 
};
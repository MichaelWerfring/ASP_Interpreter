// <copyright file="IConstructiveUnificationAlgorithm.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Unification;

using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// An interface for a constructive unification algorithm.
/// </summary>
public interface IConstructiveUnificationAlgorithm
{
    /// <summary>
    /// Attempts to unify a constructive target.
    /// </summary>
    /// <param name="target">The target to unify.</param>
    /// <returns>A result mapping in case of success, or none.</returns>
    public IOption<VariableMapping> Unify(ConstructiveTarget target);
}
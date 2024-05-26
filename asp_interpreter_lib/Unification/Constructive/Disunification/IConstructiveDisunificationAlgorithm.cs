// <copyright file="IConstructiveDisunificationAlgorithm.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Disunification;

using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Util.ErrorHandling.Either;

/// <summary>
/// An interface for a constructive disunification algorithm.
/// </summary>
public interface IConstructiveDisunificationAlgorithm
{
    /// <summary>
    /// Attempts to disunify a constructive target.
    /// </summary>
    /// <param name="target">The target to disunify.</param>
    /// <returns>Either a disunification exception or an enumerable of result mappings.</returns>
    public IEither<DisunificationException, IEnumerable<VariableMapping>> Disunify(ConstructiveTarget target);
}
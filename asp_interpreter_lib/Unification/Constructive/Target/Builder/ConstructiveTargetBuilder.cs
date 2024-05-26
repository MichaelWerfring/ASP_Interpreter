// <copyright file="ConstructiveTargetBuilder.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Target.Builder;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Util.ErrorHandling.Either;
using System.Collections.Immutable;

/// <summary>
/// A class for convenient building of a constructive target from two terms and an input mapping.
/// </summary>
public static class ConstructiveTargetBuilder
{
    private static readonly ValueRetriever Retriever = new();

    /// <summary>
    /// Builds a constructive target from two terms and an input mapping.
    /// </summary>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <param name="mapping">The input mapping.</param>
    /// <returns>Either a target building exception,
    /// in case the mapping already contained values for variables in left and right, or a constructive target.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..left is null.
    /// ..right is null.
    /// ..mapping is null.</exception>
    public static IEither<TargetBuildingException, ConstructiveTarget> Build(ISimpleTerm left, ISimpleTerm right, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        var variables = left.ExtractVariables().Union(right.ExtractVariables(), TermFuncs.GetSingletonVariableComparer());

        var newMapping = ImmutableDictionary.Create<Variable, ProhibitedValuesBinding>(TermFuncs.GetSingletonVariableComparer());

        foreach (Variable variable in variables)
        {
            mapping.TryGetValue(variable, out IVariableBinding? value);

            if (value == null)
            {
                newMapping = newMapping.Add(variable, new ProhibitedValuesBinding());
                continue;
            }

            var valueEither = Retriever.GetProhibitedValuesOrError(variable, mapping);

            if (!valueEither.IsRight)
            {
                return new Left<TargetBuildingException, ConstructiveTarget>(valueEither.GetLeftOrThrow());
            }

            newMapping = newMapping.SetItem(variable, valueEither.GetRightOrThrow());
        }

        return new Right<TargetBuildingException, ConstructiveTarget>(new ConstructiveTarget(left, right, newMapping));
    }
}
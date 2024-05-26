// <copyright file="LeftIsStructureCaseDeterminer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.RightArgumentDeterminers;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

/// <summary>
/// A class for determining the right side of a binary case when the left one is determined to be a structure.
/// </summary>
internal class LeftIsStructureCaseDeterminer : ISimpleTermArgsVisitor<IBinaryTermCase, Structure>
{
    /// <summary>
    /// Visits a term with a structure as an argument that represents the
    /// already fixated left side of the original two arguments.
    /// </summary>
    /// <param name="right">The term to visit.</param>
    /// <param name="left">The left term that has been determined already.</param>
    /// <returns>A case instance that represents the figured-out case.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is right.</exception>
    public IBinaryTermCase Visit(Variable right, Structure left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new StructureVariableCase(left, right);
    }

    /// <summary>
    /// Visits a term with a structure as an argument that represents the
    /// already fixated left side of the original two arguments.
    /// </summary>
    /// <param name="right">The term to visit.</param>
    /// <param name="left">The left term that has been determined already.</param>
    /// <returns>A case instance that represents the figured-out case.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is right.</exception>
    public IBinaryTermCase Visit(Structure right, Structure left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new StructureStructureCase(left, right);
    }

    /// <summary>
    /// Visits a term with a structure as an argument that represents the
    /// already fixated left side of the original two arguments.
    /// </summary>
    /// <param name="right">The term to visit.</param>
    /// <param name="left">The left term that has been determined already.</param>
    /// <returns>A case instance that represents the figured-out case.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is right.</exception>
    public IBinaryTermCase Visit(Integer right, Structure left)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new StructureIntegerCase(left, right);
    }
}
// <copyright file="TermCaseDeterminer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.RightArgumentDeterminers;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

/// <summary>
/// A class for determining the type case of two input values.
/// </summary>
public class TermCaseDeterminer : ISimpleTermArgsVisitor<IBinaryTermCase, ISimpleTerm>
{
    private readonly LeftIsIntegerCaseDeterminer rightCaseDeterminerForIntegerCase = new();
    private readonly LeftIsStructureCaseDeterminer rightCaseDeterminerForStructureCase = new();
    private readonly LeftIsVariableCaseDeterminer rightCaseDeterminerForVariableCase = new();

    /// <summary>
    /// Determines the concrete type case of two input terms.
    /// </summary>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <returns>The binary case of the two types.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is null.</exception>
    public IBinaryTermCase DetermineCase(ISimpleTerm left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left.Accept(this, right);
    }

    /// <summary>
    /// Visits the left term to determine its type.
    /// </summary>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <returns>The binary case of the two types.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is null.</exception>
    public IBinaryTermCase Visit(Variable left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(this.rightCaseDeterminerForVariableCase, left);
    }

    /// <summary>
    /// Visits the left term to determine its type.
    /// </summary>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <returns>The binary case of the two types.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is null.</exception>
    public IBinaryTermCase Visit(Structure left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(this.rightCaseDeterminerForStructureCase, left);
    }

    /// <summary>
    /// Visits the left term to determine its type.
    /// </summary>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <returns>The binary case of the two types.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is null.</exception>
    public IBinaryTermCase Visit(Integer left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(this.rightCaseDeterminerForIntegerCase, left);
    }
}
// <copyright file="BinaryVariableBindingCaseDeterminer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.RightCaseDeterminers;

/// <summary>
/// A class for determining the concrete type case of two input arguments of type <see cref="IVariableBinding"/>.
/// </summary>
internal class BinaryVariableBindingCaseDeterminer : IVariableBindingArgumentVisitor<IBinaryVariableBindingCase, IVariableBinding>
{
    private readonly LeftIsProhibitedValuesCaseDeterminer rightCaseDeterminerForProhibitedValuesCase = new();
    private readonly LeftIsTermBindingCaseDeterminer rightCaseDeterminerForTermBindingCase = new();

    /// <summary>
    /// Determines the case of two input arguments.
    /// </summary>
    /// <param name="left">The left argument.</param>
    /// <param name="right">The right argument.</param>
    /// <returns>A case depending on the types of the two input arguments.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="left"/> is null,
    /// ..<paramref name="right"/> is null.</exception>
    public IBinaryVariableBindingCase DetermineCase(IVariableBinding left, IVariableBinding right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left.Accept(this, right);
    }

    /// <summary>
    /// Visits the left argument to determine its type.
    /// </summary>
    /// <param name="left">The left argument.</param>
    /// <param name="right">The right argument.</param>
    /// <returns>A case depending on the types of the two input arguments.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="left"/> is null,
    /// ..<paramref name="right"/> is null.</exception>
    public IBinaryVariableBindingCase Visit(ProhibitedValuesBinding left, IVariableBinding right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(this.rightCaseDeterminerForProhibitedValuesCase, left);
    }

    /// <summary>
    /// Visits the left argument to determine its type.
    /// </summary>
    /// <param name="left">The left argument.</param>
    /// <param name="right">The right argument.</param>
    /// <returns>A case depending on the types of the two input arguments.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="left"/> is null,
    /// ..<paramref name="right"/> is null.</exception>
    public IBinaryVariableBindingCase Visit(TermBinding left, IVariableBinding right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(this.rightCaseDeterminerForTermBindingCase, left);
    }
}
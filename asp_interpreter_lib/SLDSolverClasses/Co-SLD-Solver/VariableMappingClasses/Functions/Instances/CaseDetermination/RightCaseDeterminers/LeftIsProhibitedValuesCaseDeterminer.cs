// <copyright file="LeftIsProhibitedValuesCaseDeterminer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.RightCaseDeterminers;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

/// <summary>
/// A class for figuring out the right case if left is a <see cref="ProhibitedValuesBinding"/>.
/// </summary>
internal class LeftIsProhibitedValuesCaseDeterminer : IVariableBindingArgumentVisitor<IBinaryVariableBindingCase, ProhibitedValuesBinding>
{
    /// <summary>
    /// Visits the right argument of a case determination and returns a case based on its types.
    /// </summary>
    /// <param name="right">The originally right argument.</param>
    /// <param name="left">The originally left argument.</param>
    /// <returns>A case depending on the two input types.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="right"/> is null.
    /// ..<paramref name="left"/> is null.</exception>
    public IBinaryVariableBindingCase Visit(ProhibitedValuesBinding right, ProhibitedValuesBinding left)
    {
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(left);

        return new ProhibValsProhibValsCase(left, right);
    }

    /// <summary>
    /// Visits the right argument of a case determination and returns a case based on its types.
    /// </summary>
    /// <param name="right">The originally right argument.</param>
    /// <param name="left">The originally left argument.</param>
    /// <returns>A case depending on the two input types.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="right"/> is null.
    /// ..<paramref name="left"/> is null.</exception>
    public IBinaryVariableBindingCase Visit(TermBinding right, ProhibitedValuesBinding left)
    {
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(left);

        return new ProhibValsTermBindingCase(left, right);
    }
}
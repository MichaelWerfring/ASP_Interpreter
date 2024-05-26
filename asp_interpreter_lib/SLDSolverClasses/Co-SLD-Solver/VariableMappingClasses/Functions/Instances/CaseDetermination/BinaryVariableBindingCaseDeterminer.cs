// <copyright file="BinaryVariableBindingCaseDeterminer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.RightCaseDeterminers;

internal class BinaryVariableBindingCaseDeterminer : IVariableBindingArgumentVisitor<IBinaryVariableBindingCase, IVariableBinding>
{
    private readonly LeftIsProhibitedValuesCaseDeterminer rightCaseDeterminerForProhibitedValuesCase = new();
    private readonly LeftIsTermBindingCaseDeterminer rightCaseDeterminerForTermBindingCase = new();

    public IBinaryVariableBindingCase DetermineCase(IVariableBinding left, IVariableBinding right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left.Accept(this, right);
    }

    public IBinaryVariableBindingCase Visit(ProhibitedValuesBinding left, IVariableBinding right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(this.rightCaseDeterminerForProhibitedValuesCase, left);
    }

    public IBinaryVariableBindingCase Visit(TermBinding left, IVariableBinding right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return right.Accept(this.rightCaseDeterminerForTermBindingCase, left);
    }
}
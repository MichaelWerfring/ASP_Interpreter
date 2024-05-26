// <copyright file="LeftIsProhibitedValuesCaseDeterminer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.RightCaseDeterminers;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

internal class LeftIsProhibitedValuesCaseDeterminer : IVariableBindingArgumentVisitor<IBinaryVariableBindingCase, ProhibitedValuesBinding>
{
    public IBinaryVariableBindingCase Visit(ProhibitedValuesBinding right, ProhibitedValuesBinding left)
    {
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(left);

        return new ProhibValsProhibValsCase(left, right);
    }

    public IBinaryVariableBindingCase Visit(TermBinding right, ProhibitedValuesBinding left)
    {
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(left);

        return new ProhibValsTermBindingCase(left, right);
    }
}
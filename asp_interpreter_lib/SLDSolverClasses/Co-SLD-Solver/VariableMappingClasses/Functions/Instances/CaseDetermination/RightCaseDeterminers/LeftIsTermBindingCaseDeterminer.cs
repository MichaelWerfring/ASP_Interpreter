// <copyright file="LeftIsTermBindingCaseDeterminer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.RightCaseDeterminers;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

internal class LeftIsTermBindingCaseDeterminer : IVariableBindingArgumentVisitor<IBinaryVariableBindingCase, TermBinding>
{
    public IBinaryVariableBindingCase Visit(ProhibitedValuesBinding right, TermBinding left)
    {
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(left);

        return new TermBindingProhibValsCase(left, right);
    }

    public IBinaryVariableBindingCase Visit(TermBinding right, TermBinding left)
    {
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(left);

        return new TermBindingTermBindingCase(left, right);
    }
}
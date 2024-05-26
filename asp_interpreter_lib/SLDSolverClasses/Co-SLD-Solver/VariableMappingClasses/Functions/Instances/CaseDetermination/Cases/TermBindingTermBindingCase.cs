// <copyright file="TermBindingTermBindingCase.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

public class TermBindingTermBindingCase : IBinaryVariableBindingCase
{
    public TermBindingTermBindingCase(TermBinding left, TermBinding right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        this.Left = left;
        this.Right = right;
    }

    public TermBinding Left { get; }

    public TermBinding Right { get; }

    public void Accept(IBinaryVariableBindingCaseVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        visitor.Visit(this);
    }

    public T Accept<T>(IBinaryVariableBindingCaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        return visitor.Visit(this);
    }
}
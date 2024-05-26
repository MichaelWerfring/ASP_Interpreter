﻿// <copyright file="IBinaryVariableBindingCase.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

public interface IBinaryVariableBindingCase
{
    public void Accept(IBinaryVariableBindingCaseVisitor visitor);

    public T Accept<T>(IBinaryVariableBindingCaseVisitor<T> visitor);
}
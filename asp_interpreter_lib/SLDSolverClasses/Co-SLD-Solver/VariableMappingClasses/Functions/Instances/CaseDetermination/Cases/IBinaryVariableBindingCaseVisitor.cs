// <copyright file="IBinaryVariableBindingCaseVisitor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

public interface IBinaryVariableBindingCaseVisitor
{
    public void Visit(ProhibValsProhibValsCase binaryCase);

    public void Visit(ProhibValsTermBindingCase binaryCase);

    public void Visit(TermBindingProhibValsCase binaryCase);

    public void Visit(TermBindingTermBindingCase binaryCase);
}

public interface IBinaryVariableBindingCaseVisitor<T>
{
    public T Visit(ProhibValsProhibValsCase binaryCase);

    public T Visit(ProhibValsTermBindingCase binaryCase);

    public T Visit(TermBindingProhibValsCase binaryCase);

    public T Visit(TermBindingTermBindingCase binaryCase);
}


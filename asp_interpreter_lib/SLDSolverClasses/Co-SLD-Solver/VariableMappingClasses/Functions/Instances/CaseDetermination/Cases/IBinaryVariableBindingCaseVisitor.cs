// <copyright file="IBinaryVariableBindingCaseVisitor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

/// <summary>
/// Visits a <see cref="IBinaryVariableBindingCase"/> instance.
/// </summary>
public interface IBinaryVariableBindingCaseVisitor
{
    /// <summary>
    /// Visits a case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(ProhibValsProhibValsCase binaryCase);

    /// <summary>
    /// Visits a case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(ProhibValsTermBindingCase binaryCase);

    /// <summary>
    /// Visits a case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(TermBindingProhibValsCase binaryCase);

    /// <summary>
    /// Visits a case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(TermBindingTermBindingCase binaryCase);
}

/// <summary>
/// Visits a <see cref="IBinaryVariableBindingCase"/> instance and returns a value.
/// </summary>
/// <typeparam name="T">The return type.</typeparam>
public interface IBinaryVariableBindingCaseVisitor<T>
{
    /// <summary>
    /// Visits a case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(ProhibValsProhibValsCase binaryCase);

    /// <summary>
    /// Visits a case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(ProhibValsTermBindingCase binaryCase);

    /// <summary>
    /// Visits a case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(TermBindingProhibValsCase binaryCase);

    /// <summary>
    /// Visits a case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(TermBindingTermBindingCase binaryCase);
}
// <copyright file="IBinaryVariableBindingCase.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Instances.CaseDetermination.Cases;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

/// <summary>
/// Represents a case of two <see cref="IVariableBinding"/> types.
/// </summary>
public interface IBinaryVariableBindingCase
{
    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    public void Accept(IBinaryVariableBindingCaseVisitor visitor);

    /// <summary>
    /// Accepts a visitor that returns a value.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="visitor">The visitor to accept.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Accept<T>(IBinaryVariableBindingCaseVisitor<T> visitor);
}
// <copyright file="IVariableBinding.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

/// <summary>
/// Represents the binding value of a variable.
/// </summary>
public interface IVariableBinding
{
    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    public void Accept(IVariableBindingVisitor visitor);

    /// <summary>
    /// Accepts a visitor that returns a value.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Accept<T>(IVariableBindingVisitor<T> visitor);

    /// <summary>
    /// Accepts a visitor that returns a value and has an additional argument.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="argument">The additional argument.</param>
    /// <typeparam name="TArgs">The return type.</typeparam>
    public void Accept<TArgs>(IVariableBindingArgumentVisitor<TArgs> visitor, TArgs argument);

    /// <summary>
    /// Accepts a visitor that returns a value and has an additional argument.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="argument">The additional argument.</param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <typeparam name="TArgs">The return type.</typeparam>
    /// <returns>A value of type <typeparamref name="TArgs"/>.</returns>
    public TResult Accept<TResult, TArgs>(IVariableBindingArgumentVisitor<TResult, TArgs> visitor, TArgs argument);
}
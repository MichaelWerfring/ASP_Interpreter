// <copyright file="IVariableBindingVisitor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

/// <summary>
/// Represents a visitor of a <see cref="IVariableBinding"/>.
/// </summary>
public interface IVariableBindingVisitor
{
    /// <summary>
    /// Visits a <see cref="ProhibitedValuesBinding"/>.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    void Visit(ProhibitedValuesBinding binding);

    /// <summary>
    /// Visits a <see cref="TermBinding"/>.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    void Visit(TermBinding binding);
}

/// <summary>
/// Represents a visitor of a <see cref="IVariableBinding"/> that returns a value.
/// </summary>
/// <typeparam name="T">The return type.</typeparam>
public interface IVariableBindingVisitor<T>
{
    /// <summary>
    /// Visits a <see cref="ProhibitedValuesBinding"/>.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    T Visit(ProhibitedValuesBinding binding);

    /// <summary>
    /// Visits a <see cref="TermBinding"/>.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    T Visit(TermBinding binding);
}

/// <summary>
/// Represents a visitor of a <see cref="IVariableBinding"/> that has an additional argument.
/// </summary>
/// <typeparam name="TArgs">The argument type.</typeparam>
public interface IVariableBindingArgumentVisitor<TArgs>
{
    /// <summary>
    /// Visits a <see cref="ProhibitedValuesBinding"/>.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    /// <param name="argument">The additional argument.</param>
    void Visit(ProhibitedValuesBinding binding, TArgs argument);

    /// <summary>
    /// Visits a <see cref="TermBinding"/>.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    /// <param name="argument">The additional argument.</param>
    void Visit(TermBinding binding, TArgs argument);
}

/// <summary>
/// Represents a visitor of a <see cref="IVariableBinding"/> that returns a value and has an additional argument.
/// </summary>
/// <typeparam name="TResult">The return type.</typeparam>
/// <typeparam name="TArgs">The argument type.</typeparam>
public interface IVariableBindingArgumentVisitor<TResult, TArgs>
{
    /// <summary>
    /// Visits a <see cref="ProhibitedValuesBinding"/>.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    /// <param name="argument">The additional argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    TResult Visit(ProhibitedValuesBinding binding, TArgs argument);

    /// <summary>
    /// Visits a <see cref="TermBinding"/>.
    /// </summary>
    /// <param name="binding">The binding to visit.</param>
    /// <param name="argument">The additional argument.</param>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    TResult Visit(TermBinding binding, TArgs argument);
}
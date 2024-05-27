// <copyright file="ProhibitedValuesBinding.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.Util;
using System.Collections.Immutable;

/// <summary>
/// Represents a binding of a variable to a set of prohibited values.
/// </summary>
public class ProhibitedValuesBinding : IVariableBinding
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProhibitedValuesBinding"/> class.
    /// </summary>
    public ProhibitedValuesBinding()
    {
        this.ProhibitedValues = ImmutableSortedSet.Create<ISimpleTerm>(TermFuncs.GetSingletonTermComparer());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProhibitedValuesBinding"/> class.
    /// </summary>
    /// <param name="prohibitedValuesSet">A set of prohibited values.</param>
    /// <exception cref="ArgumentException">Thrown if type of comparer for the set is not <see cref="SimpleTermComparer"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="prohibitedValuesSet"/> is null.</exception>
    public ProhibitedValuesBinding(ImmutableSortedSet<ISimpleTerm> prohibitedValuesSet)
    {
        ArgumentNullException.ThrowIfNull(prohibitedValuesSet, nameof(prohibitedValuesSet));
        if (prohibitedValuesSet.KeyComparer is not SimpleTermComparer)
        {
            throw new ArgumentException("Must contain correct comparer.");
        }

        this.ProhibitedValues = prohibitedValuesSet;
    }

    /// <summary>
    /// Gets the prohibited values.
    /// </summary>
    public ImmutableSortedSet<ISimpleTerm> ProhibitedValues { get; }

    /// <summary>
    /// Accepts a visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="visitor"/> is null.</exception>
    public void Accept(IVariableBindingVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        visitor.Visit(this);
    }

    /// <summary>
    /// Accepts a visitor that returns a value.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <typeparam name="T">The return type.</typeparam>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="visitor"/> is null.</exception>
    public T Accept<T>(IVariableBindingVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);

        return visitor.Visit(this);
    }

    /// <summary>
    /// Accepts a visitor that has an additional argument.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="argument">The additional argument.</param>
    /// <typeparam name="TArgs">The argument type.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="visitor"/> is null.</exception>
    public void Accept<TArgs>(IVariableBindingArgumentVisitor<TArgs> visitor, TArgs argument)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        ArgumentNullException.ThrowIfNull(argument);

        visitor.Visit(this, argument);
    }

    /// <summary>
    /// Accepts a visitor that has an additional argument.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    /// <param name="argument">The additional argument.</param>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <typeparam name="TArgs">The argument type.</typeparam>
    /// <returns>A value of type <typeparamref name="TResult"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="visitor"/> is null.</exception>
    public TResult Accept<TResult, TArgs>(IVariableBindingArgumentVisitor<TResult, TArgs> visitor, TArgs argument)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        ArgumentNullException.ThrowIfNull(argument);

        return visitor.Visit(this, argument);
    }

    /// <summary>
    /// Converts the <see cref="ProhibitedValuesBinding"/> to a string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        return $"ProhibitedValues:{{{this.ProhibitedValues.ToList().ListToString()}}}";
    }
}
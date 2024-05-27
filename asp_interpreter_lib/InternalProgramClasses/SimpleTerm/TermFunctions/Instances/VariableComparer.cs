// <copyright file="VariableComparer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// A class for comparing two variables. Faster than regular term comparsison.
/// </summary>
public class VariableComparer : IEqualityComparer<Variable>
{
    /// <summary>
    /// Checks if two variables are equal.
    /// </summary>
    /// <param name="x">The left variable.</param>
    /// <param name="y">The right variable.</param>
    /// <returns>A value indicating whether the two variables are equal.</returns>
    public bool Equals(Variable? x, Variable? y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null)
        {
            return false;
        }

        if (y == null)
        {
            return false;
        }

        return x.Identifier == y.Identifier;
    }

    /// <summary>
    /// Returns the hashcode for the variable.
    /// </summary>
    /// <param name="obj">The input variable.</param>
    /// <returns>The hashcode for the variable.</returns>
    /// <exception cref="ArgumentNullException">Thrown if input is null.</exception>
    public int GetHashCode([DisallowNull] Variable obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));

        return obj.Identifier.GetHashCode();
    }
}
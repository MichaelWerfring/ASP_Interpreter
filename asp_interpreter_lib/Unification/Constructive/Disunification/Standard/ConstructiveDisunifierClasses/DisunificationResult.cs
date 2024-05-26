// <copyright file="DisunificationResult.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

/// <summary>
/// A class that represents one way that a disunification can be achieved.
/// </summary>
public class DisunificationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisunificationResult"/> class.
    /// </summary>
    /// <param name="variable">The involved variable.</param>
    /// <param name="term">The involved term.</param>
    /// <param name="isInstantiation">Whether the disunification can be achieved through constraintment(eg. x \= 1)
    /// or instantiation (eg. X = 1).</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..variable is null.
    /// ..term is null.</exception>
    public DisunificationResult(Variable variable, ISimpleTerm term, bool isInstantiation)
    {
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        this.Variable = variable;
        this.Term = term;
        this.IsInstantiation = isInstantiation;
    }

    /// <summary>
    /// Gets the variable.
    /// </summary>
    public Variable Variable { get; }

    /// <summary>
    /// Gets the term.
    /// </summary>
    public ISimpleTerm Term { get; }

    /// <summary>
    /// Gets a value indicating whether the the disunification can be achieved through constraintment(eg. x \= 1)
    /// or instantiation (eg. X = 1).
    /// </summary>
    public bool IsInstantiation { get; }
}
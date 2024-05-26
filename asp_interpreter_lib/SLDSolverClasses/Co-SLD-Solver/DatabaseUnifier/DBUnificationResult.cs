// <copyright file="DBUnificationResult.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

/// <summary>
/// Represents a database unification result.
/// </summary>
public class DBUnificationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DBUnificationResult"/> class.
    /// </summary>
    /// <param name="renamedClause">The renamed clause.</param>
    /// <param name="newMapping">The new mapping after unification.</param>
    /// <param name="nextInternalIndex">The next internal variable index.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// <paramref name="renamedClause"/> is null,
    /// <paramref name="newMapping"/> is null.</exception>
    public DBUnificationResult(IEnumerable<Structure> renamedClause, VariableMapping newMapping, int nextInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(renamedClause, nameof(renamedClause));
        ArgumentNullException.ThrowIfNull(newMapping, nameof(newMapping));

        this.RenamedClause = renamedClause;
        this.NewMapping = newMapping;
        this.NextInternalIndex = nextInternalIndex;
    }

    /// <summary>
    /// Gets the renamed clause.
    /// </summary>
    public IEnumerable<Structure> RenamedClause { get; }

    /// <summary>
    /// Gets the mapping after unification.
    /// </summary>
    public VariableMapping NewMapping { get; }

    /// <summary>
    /// Gets the next internal index.
    /// </summary>
    public int NextInternalIndex { get; }
}
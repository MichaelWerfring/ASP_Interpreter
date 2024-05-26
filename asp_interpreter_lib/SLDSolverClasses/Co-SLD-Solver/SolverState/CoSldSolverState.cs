// <copyright file="CoSldSolverState.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

/// <summary>
/// Represents a solver state.
/// </summary>
public class CoSldSolverState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CoSldSolverState"/> class.
    /// </summary>
    /// <param name="currentGoals">The current goals to solve.</param>
    /// <param name="solutionState">The current solution state.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="currentGoals"/> is null,
    /// <paramref name="solutionState"/> is null.</exception>
    public CoSldSolverState(IEnumerable<Structure> currentGoals, SolutionState solutionState)
    {
        ArgumentNullException.ThrowIfNull(currentGoals, nameof(currentGoals));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));

        this.SolutionState = solutionState;
        this.CurrentGoals = currentGoals;
    }

    /// <summary>
    /// Gets the current goals.
    /// </summary>
    public IEnumerable<Structure> CurrentGoals { get; }

    /// <summary>
    /// Gets the current solution state.
    /// </summary>
    public SolutionState SolutionState { get; }
}
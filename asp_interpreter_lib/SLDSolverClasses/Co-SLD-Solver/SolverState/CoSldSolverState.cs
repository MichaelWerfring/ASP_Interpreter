// <copyright file="CoSldSolverState.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

public class CoSldSolverState
{
    public CoSldSolverState(IEnumerable<Structure> currentGoals, SolutionState solutionState)
    {
        ArgumentNullException.ThrowIfNull(currentGoals, nameof(currentGoals));
        ArgumentNullException.ThrowIfNull(solutionState, nameof(solutionState));

        SolutionState = solutionState;
        CurrentGoals = currentGoals;
    }

    public IEnumerable<Structure> CurrentGoals { get; }

    public SolutionState SolutionState { get; }
}

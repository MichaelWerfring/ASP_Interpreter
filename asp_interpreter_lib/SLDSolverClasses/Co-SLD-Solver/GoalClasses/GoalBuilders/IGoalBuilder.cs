// <copyright file="IGoalBuilder.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

/// <summary>
/// An interface for a term builder.
/// </summary>
public interface IGoalBuilder
{
    /// <summary>
    /// Builds a term.
    /// </summary>
    /// <param name="term">The current term structure.</param>
    /// <param name="state">The current state of the solver.</param>
    /// <returns>A goal to resolve.</returns>
    public ICoSLDGoal BuildGoal(Structure term, SolutionState state);
}
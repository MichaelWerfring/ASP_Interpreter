// <copyright file="ICoSLDGoal.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

/// <summary>
/// Represents a goal to solve.
/// </summary>
public interface ICoSLDGoal
{
    /// <summary>
    /// Attempts to solve the goal.
    /// </summary>
    /// <returns>All the ways the goal can be solved.</returns>
    public IEnumerable<GoalSolution> TrySatisfy();
}
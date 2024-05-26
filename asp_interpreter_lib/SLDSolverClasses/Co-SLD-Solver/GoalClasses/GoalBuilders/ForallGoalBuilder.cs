// <copyright file="ForallGoalBuilder.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class that represents a builder for a forall goal.
/// </summary>
public class ForallGoalBuilder : IGoalBuilder
{
    private readonly ILogger logger;
    private readonly GoalSolver solverForForallGoal;
    private readonly int maxSolutionCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="ForallGoalBuilder"/> class.
    /// </summary>
    /// <param name="logger">The logger for the goals.</param>
    /// <param name="solver">The solver that the forall goals will use.</param>
    /// <param name="maxSolutionCount">A maximum solution count to return.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="logger"/> is null,
    /// ..<paramref name="solver"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxSolutionCount"/> is less than 1.</exception>
    public ForallGoalBuilder(ILogger logger, GoalSolver solver, int maxSolutionCount)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(solver, nameof(solver));
        ArgumentOutOfRangeException.ThrowIfLessThan(maxSolutionCount, 1);

        this.logger = logger;
        this.solverForForallGoal = solver;
        this.maxSolutionCount = maxSolutionCount;
    }

    /// <summary>
    /// Builds a goal based on a goal term and the solver's state.
    /// </summary>
    /// <param name="goalTerm">The term.</param>
    /// <param name="state">The current state.</param>
    /// <returns>A goal.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="goalTerm"/> is null,
    /// ..<paramref name="state"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if..
    /// ..goal does not have two children,
    /// ..goals first child is not a variable,
    /// ..goals second child is not a structure.</exception>
    public ICoSLDGoal BuildGoal(Structure goalTerm, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(goalTerm);
        ArgumentNullException.ThrowIfNull(state);

        if (goalTerm.Children.Count != 2)
        {
            throw new ArgumentException("Next goal must be a structure term with two children.", nameof(goalTerm));
        }

        var variableAtPositionOneMaybe = TermFuncs.ReturnVariableOrNone(goalTerm.Children[0]);
        if (!variableAtPositionOneMaybe.HasValue)
        {
            throw new ArgumentException("First child must be a variable.");
        }

        var structureAtPositionTwoMaybe = TermFuncs.ReturnStructureOrNone(goalTerm.Children[1]);
        if (!structureAtPositionTwoMaybe.HasValue)
        {
            throw new ArgumentException("Second child must be a structure.");
        }

        return new ForallGoal(
            this.solverForForallGoal,
            variableAtPositionOneMaybe.GetValueOrThrow(),
            structureAtPositionTwoMaybe.GetValueOrThrow(),
            state,
            this.logger,
            this.maxSolutionCount);
    }
}
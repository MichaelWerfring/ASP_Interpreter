// <copyright file="DisunificationGoalBuilder.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.Unification.Constructive.Disunification;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Unification.Constructive.Target.Builder;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class that represents a builder for a disunification goal.
/// </summary>
public class DisunificationGoalBuilder : IGoalBuilder
{
    private readonly SolverStateUpdater stateUpdater;
    private readonly IConstructiveDisunificationAlgorithm algorithm;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DisunificationGoalBuilder"/> class.
    /// </summary>
    /// <param name="updater">The solver state updater for the goals.</param>
    /// <param name="algorithm">The algorithm for the goals.</param>
    /// <param name="logger">The logger for the goals.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="updater"/> is null,
    /// <paramref name="algorithm"/> is null,
    /// <paramref name="logger"/> is null.</exception>
    public DisunificationGoalBuilder(SolverStateUpdater updater, IConstructiveDisunificationAlgorithm algorithm, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(updater);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        this.stateUpdater = updater;
        this.algorithm = algorithm;
        this.logger = logger;
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
    /// ..mapping of <paramref name="state"/> contains term bindings.</exception>
    public ICoSLDGoal BuildGoal(Structure goalTerm, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(goalTerm);
        ArgumentNullException.ThrowIfNull(state);

        if (goalTerm.Children.Count != 2)
        {
            throw new ArgumentException("Next goal must be a structure term with two children.", nameof(goalTerm));
        }

        var targetEither = ConstructiveTargetBuilder.Build(
           goalTerm.Children.ElementAt(0),
           goalTerm.Children.ElementAt(1),
           state.Mapping);

        ConstructiveTarget target;
        try
        {
            target = targetEither.GetRightOrThrow();
        }
        catch
        {
            throw new ArgumentException($"{nameof(state.Mapping)} contained term bindings : {targetEither.GetLeftOrThrow().Message}");
        }

        return new DisunificationGoal(this.stateUpdater, target, this.algorithm, state, this.logger);
    }
}
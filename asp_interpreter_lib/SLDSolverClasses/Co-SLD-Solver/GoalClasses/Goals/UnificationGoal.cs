// <copyright file="UnificationGoal.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Unification.Constructive.Unification;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// Represents a unification goal.
/// </summary>
public class UnificationGoal : ICoSLDGoal
{
    private readonly SolverStateUpdater updater;
    private readonly ConstructiveTarget target;
    private readonly IConstructiveUnificationAlgorithm algorithm;
    private readonly SolutionState state;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnificationGoal"/> class.
    /// </summary>
    /// <param name="updater">A state updater.</param>
    /// <param name="target">The disunification target.</param>
    /// <param name="algorithm">The disunification algorithm.</param>
    /// <param name="state">The input solution state.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="updater"/> is null,
    /// ..<paramref name="target"/> is null,
    /// ..<paramref name="algorithm"/> is null,
    /// ..<paramref name="state"/> is null,
    /// ..<paramref name="logger"/> is null.</exception>
    public UnificationGoal(
        SolverStateUpdater updater,
        ConstructiveTarget target,
        IConstructiveUnificationAlgorithm algorithm,
        SolutionState state,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(updater, nameof(updater));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        this.updater = updater;
        this.target = target;
        this.algorithm = algorithm;
        this.state = state;
        this.logger = logger;
    }

    /// <summary>
    /// Attempts to solve the goal.
    /// </summary>
    /// <returns>An enumeration of all the ways the goal can be solved.</returns>
    public IEnumerable<GoalSolution> TrySatisfy()
    {
        this.logger.LogInfo($"Attempting to solve unification goal: {this.target}");
        this.logger.LogTrace($"Input state is: {this.state}");

        var unificationMaybe = this.algorithm.Unify(this.target);
        if (!unificationMaybe.HasValue)
        {
            yield break;
        }

        this.logger.LogInfo($"Solved unification goal: {this.target}");

        var unifyingMapping = unificationMaybe.GetValueOrThrow();
        this.logger.LogTrace($"Unifying mapping is {unifyingMapping}");

        var updatedMapping = this.state.Mapping.Update(unifyingMapping).GetValueOrThrow();
        this.logger.LogTrace($"Updated mapping is {updatedMapping}");

        var flattenedMapping = updatedMapping.Flatten();
        this.logger.LogTrace($"Flattened mapping is {updatedMapping}");

        CoinductiveHypothesisSet updatedCHS = this.updater.UpdateCHS(this.state.CHS, flattenedMapping);
        this.logger.LogTrace($"Updated CHS is {updatedCHS}");

        CallStack updatedCallstack = this.updater.UpdateCallstack(this.state.Callstack, flattenedMapping);
        this.logger.LogTrace($"Updated callstack is {updatedCallstack}");

        yield return new GoalSolution(
            updatedCHS,
            flattenedMapping,
            updatedCallstack,
            this.state.NextInternalVariableIndex);
    }
}
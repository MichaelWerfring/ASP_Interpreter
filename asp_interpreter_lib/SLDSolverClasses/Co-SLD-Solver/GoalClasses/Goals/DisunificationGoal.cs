// <copyright file="DisunificationGoal.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

using Asp_interpreter_lib.Unification.Constructive.Disunification;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.Util.ErrorHandling.Either;
using Asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;

/// <summary>
/// Represents a disunification goal.
/// </summary>
public class DisunificationGoal : ICoSLDGoal
{
    private readonly SolverStateUpdater updater;
    private readonly ConstructiveTarget target;

    private readonly IConstructiveDisunificationAlgorithm algorithm;

    private readonly SolutionState inputState;

    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DisunificationGoal"/> class.
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
    public DisunificationGoal(
        SolverStateUpdater updater,
        ConstructiveTarget target,
        IConstructiveDisunificationAlgorithm algorithm,
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
        this.inputState = state;
        this.logger = logger;
    }

    /// <summary>
    /// Attempts to solve the goal.
    /// </summary>
    /// <returns>An enumeration of all the ways the goal can be solved.</returns>
    public IEnumerable<GoalSolution> TrySatisfy()
    {
        this.logger.LogInfo($"Attempting to solve disunification goal: {this.target}");
        this.logger.LogTrace($"Input state is: {this.inputState}");

        IEither<DisunificationException, IEnumerable<VariableMapping>> disunificationsResult = this.algorithm.Disunify(this.target);

        if (!disunificationsResult.IsRight)
        {
            yield break;
        }

        foreach (VariableMapping disunifyingMapping in disunificationsResult.GetRightOrThrow())
        {
            this.logger.LogInfo($"Solved disunification goal: {this.target} with {disunifyingMapping}");

            this.logger.LogTrace($"Disunifying mapping is {disunifyingMapping}");

            VariableMapping updatedMapping = this.inputState.Mapping.Update(disunifyingMapping).GetValueOrThrow();
            this.logger.LogTrace($"Updated mapping is {updatedMapping}");

            VariableMapping flattenedMapping = updatedMapping.Flatten();
            this.logger.LogTrace($"Flattened mapping is {updatedMapping}");

            CoinductiveHypothesisSet updatedCHS = this.updater.UpdateCHS(this.inputState.CHS, flattenedMapping);
            this.logger.LogTrace($"updated CHS is {updatedCHS}");

            CallStack updatedCallstack = this.updater.UpdateCallstack(this.inputState.Callstack, flattenedMapping);
            this.logger.LogTrace($"updated callstack is {updatedCallstack}");

            yield return new GoalSolution(
                updatedCHS,
                flattenedMapping,
                updatedCallstack,
                this.inputState.NextInternalVariableIndex);
        }
    }
}
// <copyright file="CoSLDGoalMapper.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.Database;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.GoalBuilders;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using Asp_interpreter_lib.Unification.Constructive.Disunification.Standard;
using Asp_interpreter_lib.Unification.Constructive.Unification.Standard;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

/// <summary>
/// A class for mapping an input structure to a the appropriate goal.
/// </summary>
public class CoSLDGoalMapper
{
    private readonly ImmutableDictionary<(string, int), IGoalBuilder> mapping;

    private readonly PredicateGoalBuilder predicateGoalBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoSLDGoalMapper"/> class.
    /// </summary>
    /// <param name="functorTable">Contains the functors that will be used to identify special goals.</param>
    /// <param name="database">A database to use by a predicate goal.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="functorTable"/> is null,
    /// ..<paramref name="database"/> is null,
    /// ..<paramref name="logger"/> is null.</exception>
    public CoSLDGoalMapper(FunctorTableRecord functorTable, IDatabase database, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(functorTable);
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(logger);

        var goalBuilderDict = new Dictionary<(string, int), IGoalBuilder>()
        {
            {
                (functorTable.ArithmeticEvaluation, 2),
                new ArithmeticEvaluationGoalBuilder(
                    new SolverStateUpdater(),
                    new ArithmeticEvaluator(functorTable),
                    new StandardConstructiveUnificationAlgorithm(false),
                    logger)
            },
            {
                (functorTable.Unification, 2),
                new UnificationGoalBuilder(new(), new StandardConstructiveUnificationAlgorithm(false), logger)
            },
            {
                (functorTable.Disunification, 2),
                new DisunificationGoalBuilder(new(), new StandardConstructiveDisunificationAlgorithm(true, false), logger)
            },
            {
                (functorTable.LessThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left < right, new ArithmeticEvaluator(functorTable), logger)
            },
            {
                (functorTable.LessOrEqualThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left <= right, new ArithmeticEvaluator(functorTable), logger)
            },
            {
                (functorTable.GreaterThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left > right, new ArithmeticEvaluator(functorTable), logger)
            },
            {
                (functorTable.GreaterOrEqualThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left >= right, new ArithmeticEvaluator(functorTable), logger)
            },
            {
                (functorTable.Forall, 2), new ForallGoalBuilder(logger ,new GoalSolver(this), 100)
            },
            {
                (functorTable.ArithmeticEvaluationNegated, 2),
                new NegatedArithmeticEvaluationGoalBuilder(
                    new ArithmeticEvaluator(functorTable),
                    new StandardConstructiveDisunificationAlgorithm(false, false),
                    logger)
            },
        };

        this.mapping = goalBuilderDict.ToImmutableDictionary();

        this.predicateGoalBuilder = new PredicateGoalBuilder(
            new CoinductiveChecker(
                new CHSChecker(
                    new ExactMatchChecker(new StandardConstructiveUnificationAlgorithm(false)),
                    new StandardConstructiveUnificationAlgorithm(false),
                    functorTable,
                    new GoalSolver(this),
                    logger),
                new CallstackChecker(
                    functorTable,
                    new ExactMatchChecker(new StandardConstructiveUnificationAlgorithm(true)),
                    new StandardConstructiveUnificationAlgorithm(false),
                    logger)),
            new DatabaseUnifier(new StandardConstructiveUnificationAlgorithm(false), database, logger),
            new GoalSolver(this),
            new PredicateGoalStateUpdater(new SolverStateUpdater()),
            logger);
    }

    /// <summary>
    /// Builds a goal based on the current state of a solver.
    /// </summary>
    /// <param name="state">The state of the solver.</param>
    /// <returns>A goal-</returns>
    /// <exception cref="ArgumentException">Thrown if state does not contain at least one goal.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is null.</exception>
    public ICoSLDGoal GetGoal(CoSldSolverState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (!state.CurrentGoals.Any())
        {
            throw new ArgumentException($"{nameof(state)} must contain at least one term in {nameof(state.CurrentGoals)}", nameof(state));
        }

        Structure goalTerm = state.CurrentGoals.First();

        this.mapping.TryGetValue((goalTerm.Functor, goalTerm.Children.Count), out IGoalBuilder? goalBuilder);

        if (goalBuilder == null)
        {
            return this.predicateGoalBuilder.BuildGoal(goalTerm, state.SolutionState);
        }

        return goalBuilder.BuildGoal(goalTerm, state.SolutionState);
    }
}
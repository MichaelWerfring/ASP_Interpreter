// <copyright file="CoinductiveSLDSolver.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.Database;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for coinductive solving of queries, in the style outlined in the s(ASP) paper.
/// </summary>
public class CoinductiveSLDSolver
{
    private readonly GoalSolver goalSolver;

    private readonly SolutionPostprocessor postprocessor;

    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoinductiveSLDSolver"/> class.
    /// </summary>
    /// <param name="database">A database to query against.</param>
    /// <param name="functorTable">The functor table to use.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="database"/> is null,
    /// ..<paramref name="functorTable"/> is null,
    /// ..<paramref name="logger"/> is null.</exception>
    public CoinductiveSLDSolver(IDatabase database, FunctorTableRecord functorTable, SolutionPostprocessor postprocessor, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(database, nameof(database));
        ArgumentNullException.ThrowIfNull(functorTable, nameof(functorTable));
        ArgumentNullException.ThrowIfNull(postprocessor, nameof(postprocessor));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        this.goalSolver = new GoalSolver(new CoSLDGoalMapper(functorTable, database, logger));

        this.postprocessor = postprocessor;

        this.logger = logger;
    }

    /// <summary>
    /// Attempts to solve a query.
    /// </summary>
    /// <param name="query">The query to solve.</param>
    /// <returns>An enumeration of solutions.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="query"/> is null.</exception>
    public IEnumerable<CoSLDSolution> Solve(IEnumerable<Structure> query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        var initialSolverState = new CoSldSolverState(query, new SolutionState([], [], [], 0));

        foreach (var querySolution in this.goalSolver.SolveGoals(initialSolverState))
        {
            this.logger.LogInfo($"Found Solution: {querySolution.ResultSet.ToList().ListToString()}");
            this.logger.LogDebug($"Mapping for Solution: {querySolution.ResultMapping}");

            var postprocessedSolution = this.postprocessor.Postprocess(querySolution);

            yield return postprocessedSolution;
        }
    }
}
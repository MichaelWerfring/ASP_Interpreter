﻿using asp_interpreter_lib.FunctorNaming;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;

public class CoinductiveSLDSolver
{
    private readonly GoalSolver _goalSolver;

    private readonly SolutionPostprocessor _postprocessor;

    private readonly ILogger _logger;

    public CoinductiveSLDSolver(IDatabase database, FunctorTableRecord functors, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(database, nameof(database));
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _goalSolver = new GoalSolver(new CoSLDGoalMapper(functors, database, logger), logger);

        _postprocessor = new SolutionPostprocessor(new VariableMappingPostprocessor(), new CHSPostProcessor());

        _logger = logger;
    }

    public IEnumerable<CoSLDSolution> Solve(IEnumerable<ISimpleTerm> query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        var initialSolverState = new CoSldSolverState(query, new SolutionState([], [], [], 0));

        foreach (var querySolution in _goalSolver.SolveGoals(initialSolverState))
        {
            _logger.LogInfo("Found Solution: { " + querySolution.ResultSet.ToList().ListToString() + " }");
            _logger.LogDebug("Mapping for Solution: " + AspExtensions.SimplifyMapping(querySolution.ResultMapping));

            var postprocessedSolution = _postprocessor.Postprocess(querySolution);

            yield return postprocessedSolution;
        }
    }
}

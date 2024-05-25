﻿using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public class ArithmeticEvaluationGoalBuilder : IGoalBuilder
{
    private readonly ArithmeticEvaluator _evaluator;
    private readonly IConstructiveUnificationAlgorithm _algorithm;
    private readonly ILogger _logger;
    private readonly SolverStateUpdater _updater;

    public ArithmeticEvaluationGoalBuilder
    (
        SolverStateUpdater updater,
        ArithmeticEvaluator evaluator,
        IConstructiveUnificationAlgorithm algorithm,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(updater);
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        _evaluator = evaluator;
        _algorithm = algorithm;
        _logger = logger;
        _updater = updater;
    }

    public ICoSLDGoal BuildGoal(Structure goalTerm, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(goalTerm);
        ArgumentNullException.ThrowIfNull(state);

        if (goalTerm.Children.Count != 2)
        {
            throw new ArgumentException("Goal must be a structure term with two children.", nameof(goalTerm)); 
        }

        return new ArithmeticEvaluationGoal
        (
            _updater,
            _evaluator,
            goalTerm.Children.ElementAt(0),
            goalTerm.Children.ElementAt(1),
            state,
            _algorithm,
            _logger
        );
    }
}
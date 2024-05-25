﻿using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.Unification.Constructive.Disunification;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.GoalBuilders;

internal class NegatedArithmeticEvaluationGoalBuilder : IGoalBuilder
{
    private readonly ArithmeticEvaluator _evaluator;
    private readonly IConstructiveDisunificationAlgorithm _algorithm;
    private readonly ILogger _logger;

    public NegatedArithmeticEvaluationGoalBuilder
    (
        ArithmeticEvaluator evaluator,
        IConstructiveDisunificationAlgorithm algorithm,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        _evaluator = evaluator;
        _algorithm = algorithm;
        _logger = logger;
    }

    public ICoSLDGoal BuildGoal(Structure goalTerm, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(goalTerm);
        ArgumentNullException.ThrowIfNull(state);

        if (goalTerm.Children.Count != 2)
        {
            throw new ArgumentException("Next goal must be a structure term with two children.", nameof(goalTerm));
        }

        return new NegatedArithmeticEvaluationGoal
        (
            _evaluator,
            goalTerm.Children.ElementAt(0),
            goalTerm.Children.ElementAt(1),
            state,
            _algorithm,
            _logger
        );
    }
}

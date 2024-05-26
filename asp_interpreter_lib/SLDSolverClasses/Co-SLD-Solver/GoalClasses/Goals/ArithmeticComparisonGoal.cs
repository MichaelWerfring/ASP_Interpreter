// <copyright file="ArithmeticComparisonGoal.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.Comparison;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class ArithmeticComparisonGoal : ICoSLDGoal
{
    private readonly ArithmeticEvaluator _evaluator;
    private readonly ISimpleTerm _left;
    private readonly ISimpleTerm _right;
    private readonly Func<int, int, bool> _predicate;
    private readonly SolutionState _inputstate;
    private readonly ILogger _logger;

    public ArithmeticComparisonGoal
    (
        ArithmeticEvaluator evaluator,
        ISimpleTerm left,
        ISimpleTerm right,
        Func<int, int, bool> predicate,
        SolutionState solutionState,
        ILogger logger
    )
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(solutionState);
        ArgumentNullException.ThrowIfNull(logger);

        _evaluator = evaluator;
        _left = left;
        _right = right;
        _predicate = predicate;
        _inputstate = solutionState;
        _logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        _logger.LogInfo($"Attempting to solve arithmetic comparison goal: {_left}, {_right}");
        _logger.LogTrace($"Input state is: {_inputstate}");

        var leftIntegerMaybe = TermFuncs.ReturnIntegerOrNone( _left );
        if (!leftIntegerMaybe.HasValue)
        {
            yield break;
        }

        var leftInteger = leftIntegerMaybe.GetValueOrThrow();

        var rightEvaluationMaybe = _evaluator.Evaluate(_right);

        if (!rightEvaluationMaybe.HasValue)
        {
            yield break;
        }
       
        if (!_predicate(leftInteger.Value, rightEvaluationMaybe.GetValueOrThrow()))
        {
            yield break;
        }

        _logger.LogInfo($"Solved arithmetic comparison goal: {_left}, {_right}");

        yield return new GoalSolution
        (
            _inputstate.CHS, 
            _inputstate.Mapping, 
            _inputstate.Callstack,
            _inputstate.NextInternalVariableIndex
        );
    }
}
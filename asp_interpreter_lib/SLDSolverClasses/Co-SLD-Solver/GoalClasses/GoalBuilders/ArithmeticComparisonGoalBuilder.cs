using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals.ArithmeticEvaluation;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.Comparison;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.GoalBuilders;

internal class ArithmeticComparisonGoalBuilder : IGoalBuilder
{
    private readonly Func<int, int, bool> _predicate;
    private readonly ArithmeticEvaluator _evaluator;

    public ArithmeticComparisonGoalBuilder(Func<int, int, bool> predicate, ArithmeticEvaluator evaluator)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(evaluator);

        _predicate = predicate;
        _evaluator = evaluator;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        if(currentState.CurrentGoals.Count() == 0)
        { throw new ArgumentException("Must contain at least one term!", nameof(currentState.CurrentGoals)); }

        var goalTerm = currentState.CurrentGoals.First();
        if(goalTerm is not Structure comparisonStruct || comparisonStruct.Children.Count() != 2)
        { throw new ArgumentException("Must contain a structure term with two children.", nameof(currentState.CurrentGoals)); }

        return new ArithmeticComparisonGoal
        (
            _evaluator,
            comparisonStruct.Children.ElementAt(0),
            comparisonStruct.Children.ElementAt(1),
            _predicate,
            currentState.CurrentGoals.Skip(1).ToImmutableList(),
            currentState.SolutionState
        );
    }
}

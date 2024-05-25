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

public class CoSLDGoalMapper
{
    private readonly ImmutableDictionary<(string, int), IGoalBuilder> _mapping;

    private readonly PredicateGoalBuilder _predicateGoalBuilder;

    private readonly ILogger _logger;

    public CoSLDGoalMapper(FunctorTableRecord functors, IDatabase database, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(functors);
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(logger);

        var goalBuilderDict = new Dictionary<(string, int), IGoalBuilder>()
        {
            {
                (functors.ArithmeticEvaluation, 2),
                new ArithmeticEvaluationGoalBuilder
                (
                    new SolverStateUpdater(),
                    new ArithmeticEvaluator(functors), 
                    new StandardConstructiveUnificationAlgorithm(false), logger
                )
            },
            {
                (functors.Unification, 2),
                new UnificationGoalBuilder(new(), new StandardConstructiveUnificationAlgorithm(false), logger)
            },
            {
                (functors.Disunification, 2),
                new DisunificationGoalBuilder(new(), new StandardConstructiveDisunificationAlgorithm(true, false), logger)
            },
            {
                (functors.LessThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left < right, new ArithmeticEvaluator(functors), logger)
            },
            {
                (functors.LessOrEqualThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left <= right, new ArithmeticEvaluator(functors), logger)
            },
            {
                (functors.GreaterThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left > right, new ArithmeticEvaluator(functors), logger)
            },
            {
                (functors.GreaterOrEqualThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left >= right, new ArithmeticEvaluator(functors), logger)
            },
            {
                (functors.Forall, 2), new ForallGoalBuilder(logger ,new GoalSolver(this, logger), 1)
            },
            {
                (functors.ArithmeticEvaluationNegated, 2),
                new NegatedArithmeticEvaluationGoalBuilder(new ArithmeticEvaluator(functors),
                new StandardConstructiveDisunificationAlgorithm(false, false), logger) 
            }
        };

        _mapping = goalBuilderDict.ToImmutableDictionary();

        _predicateGoalBuilder = new PredicateGoalBuilder
         (
            new CoinductiveChecker
            (
                new CHSChecker
                (
                    new ExactMatchChecker(new StandardConstructiveUnificationAlgorithm(false)),
                    new StandardConstructiveUnificationAlgorithm(false),
                    functors, 
                    new GoalSolver(this, logger),
                    logger
                ),
                new CallstackChecker
                (
                    functors,
                    new ExactMatchChecker(new StandardConstructiveUnificationAlgorithm(true)), 
                    new StandardConstructiveUnificationAlgorithm(false),
                    logger
                )
            ),
            new DatabaseUnifier(new StandardConstructiveUnificationAlgorithm(false), database, logger),
            new GoalSolver(this, logger),
            new PredicateGoalStateUpdater(new SolverStateUpdater()),
            logger
         );

        _logger = logger;
    }

    public IOption<ICoSLDGoal> GetGoal(CoSldSolverState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (!state.CurrentGoals.Any())
        {
            throw new ArgumentException($"{nameof(state)} must contain at least one term in {nameof(state.CurrentGoals)}", nameof(state));
        }

        Structure goalTerm = state.CurrentGoals.First();

        _mapping.TryGetValue((goalTerm.Functor, goalTerm.Children.Count), out IGoalBuilder? goalBuilder);

        if (goalBuilder == null)
        {
            return new Some<ICoSLDGoal>(_predicateGoalBuilder.BuildGoal(goalTerm, state.SolutionState));
        }

        return new Some<ICoSLDGoal>(goalBuilder.BuildGoal(goalTerm, state.SolutionState));
    }
}
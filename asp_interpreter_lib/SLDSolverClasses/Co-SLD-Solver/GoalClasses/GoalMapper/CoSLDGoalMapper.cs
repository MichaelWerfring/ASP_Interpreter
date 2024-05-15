﻿using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.GoalBuilders;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;
using asp_interpreter_lib.Unification.Constructive.Disunification.Standard;
using asp_interpreter_lib.Unification.Constructive.Unification.Standard;
using asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class CoSLDGoalMapper : ISimpleTermArgsVisitor<IOption<ICoSLDGoal>, (CoSldSolverState, IDatabase)>
{
    private readonly IImmutableDictionary<(string, int), IGoalBuilder> _mapping;

    private readonly PredicateGoalBuilder _dbGoalBuilder;

    private readonly ILogger _logger;

    public CoSLDGoalMapper(FunctorTableRecord functors, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(functors);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;

        var goalBuilderDict = new Dictionary<(string, int), IGoalBuilder>()
        {
            {
                (functors.ArithmeticEvaluation, 2),
                new ArithmeticEvaluationGoalBuilder(new ArithmeticEvaluator(functors), new StandardConstructiveUnificationAlgorithm(false))
            },
            {
                (functors.Unification, 2),
                new UnificationGoalBuilder(new StandardConstructiveUnificationAlgorithm(false))
            },
            {
                (functors.Disunification, 2),
                new DisunificationGoalBuilder(new StandardConstructiveDisunificationAlgorithm(true, false))
            },
            {
                (functors.LessThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left < right, new ArithmeticEvaluator(functors))
            },
            {
                (functors.LessOrEqualThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left <= right, new ArithmeticEvaluator(functors))
            },
            {
                (functors.GreaterThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left > right, new ArithmeticEvaluator(functors))
            },
            {
                (functors.GreaterOrEqualThan, 2),
                new ArithmeticComparisonGoalBuilder((left, right) => left >= right, new ArithmeticEvaluator(functors))
            },
            {
                (functors.Forall, 2), new ForallGoalBuilder(functors, _logger)
            }
        };

        _mapping = goalBuilderDict.ToImmutableDictionary();
        _dbGoalBuilder = new PredicateGoalBuilder
         (
            this,
            new StandardConstructiveUnificationAlgorithm(false),
            new PredicateGoalStateUpdater(new SolverStateUpdater()),
            _logger
         );
    }

    public IOption<ICoSLDGoal> GetGoal(CoSldSolverState state, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(database);
        ISimpleTerm? currentGoalTerm;
        try
        {
            currentGoalTerm = state.CurrentGoals.First();
        }
        catch
        {
            throw new ArgumentException($"{nameof(state)} must contain at least one term in {nameof(state.CurrentGoals)}", nameof(state));
        }

        return currentGoalTerm.Accept(this, (state, database));
    }

    public IOption<ICoSLDGoal> Visit(Integer integer, (CoSldSolverState, IDatabase) arguments)
    {
        return new None<ICoSLDGoal>();
    }

    public IOption<ICoSLDGoal> Visit(Variable variableTerm, (CoSldSolverState, IDatabase) arguments)
    {
        return new None<ICoSLDGoal>();
    }

    public IOption<ICoSLDGoal> Visit(Structure basicTerm, (CoSldSolverState, IDatabase) arguments)
    {
        IGoalBuilder? goalBuilder;
        _mapping.TryGetValue((basicTerm.Functor, basicTerm.Children.Count()), out goalBuilder);

        if(goalBuilder != null)
        {
            return new Some<ICoSLDGoal>(goalBuilder.BuildGoal(arguments.Item1, arguments.Item2));
        }

        return new Some<ICoSLDGoal>(_dbGoalBuilder.BuildGoal(arguments.Item1, arguments.Item2));
    }
}

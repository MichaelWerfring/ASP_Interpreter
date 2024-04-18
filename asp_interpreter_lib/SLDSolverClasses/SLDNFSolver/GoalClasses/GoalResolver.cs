﻿using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalClasses.Goals.Comparison;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalClasses.Goals.Unification;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.GoalMapping;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals.ArithmeticEvaluation;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Unification.Robinson;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication;

public class GoalResolver
{
    private GoalMapper _goalmapper;

    public GoalResolver(FunctorTableRecord specialFunctors)
    {
        ArgumentNullException.ThrowIfNull(specialFunctors);

        var dict = new Dictionary<(string, int), IGoal>()
        {
            {(specialFunctors.ArithmeticEvaluation, 2), new ArithmeticEvaluationGoal(specialFunctors) },

            {(specialFunctors.GreaterOrEqualThan, 2), new GreaterThanOrEqualGoal(specialFunctors) },
            {(specialFunctors.GreaterThan, 2), new GreaterThanGoal(specialFunctors) },
            {(specialFunctors.LessOrEqualThan, 2), new SmallerThanOrEqualGoal(specialFunctors) },
            {(specialFunctors.LessThan, 2), new SmallerThanGoal(specialFunctors) },

            {(specialFunctors.Disunification, 2), new DisunificationGoal(new RobinsonUnificationAlgorithm(false)) },
            {(specialFunctors.Unification, 2), new UnificationGoal(new RobinsonUnificationAlgorithm(false)) },

            {(specialFunctors.NegationAsFailure, 1), new NafGoal(specialFunctors) },
        };

        _goalmapper = new GoalMapper(dict);
    }

    public IEnumerable<SolverState> Solve(SolverState state, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(database, nameof(database));
        if(state.CurrentGoals.Count() < 1)
        {
            throw new Exception("Must contain at least one goal.");
        }

        ISimpleTerm goalTerm = state.CurrentGoals.First();

        var goal = _goalmapper.GetGoal(goalTerm);
        if (!goal.HasValue)
        {
            yield break;
        }

        var branches = goal.GetValueOrThrow().TrySatisfy(database, state);

        foreach(var branch in branches)
        {
            yield return branch;
        };
    }
}

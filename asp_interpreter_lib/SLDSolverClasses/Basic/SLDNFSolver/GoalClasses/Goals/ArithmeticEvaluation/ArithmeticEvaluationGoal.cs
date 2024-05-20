using asp_interpreter_lib.FunctorNaming;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals.ArithmeticEvaluation;

public class ArithmeticEvaluationGoal : IGoal
{
    private ArithmeticEvaluator _evaluator;

    public ArithmeticEvaluationGoal(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable);

        _evaluator = new ArithmeticEvaluator(functorTable);
    }

    public IEnumerable<SolverState> TrySatisfy(IDatabase database, SolverState state)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(state);

        if (state.CurrentGoals.Count() < 1) { throw new ArgumentException(nameof(state)); }
        Structure evaluation;
        try
        {
            evaluation = (Structure)state.CurrentGoals.First();
        }
        catch
        {
            throw new ArgumentException("Must be a structure.");
        }
        if (evaluation.Children.Count() != 2)
        {
            throw new ArgumentException(nameof(state));
        }

        var rightEvaluationMaybe = _evaluator.Evaluate(evaluation.Children.ElementAt(1));
        int rightEvaluation;
        try
        {
            rightEvaluation = rightEvaluationMaybe.GetValueOrThrow();
        }
        catch
        {
            return [];
        }

        if (evaluation.Children.ElementAt(0) is Variable leftVariable)
        {
            Dictionary<Variable, ISimpleTerm> substitution = new Dictionary<Variable, ISimpleTerm>(TermFuncs.GetSingletonVariableComparer())
            {
                {leftVariable, new Integer(rightEvaluation) }
            };

            var newGoals = state.CurrentGoals.Skip(1).Select((term) => term.Substitute(substitution));

            return
            [
                new SolverState
                (
                    newGoals,
                    state.CurrentSubstitution.Union(substitution).ToDictionary(TermFuncs.GetSingletonVariableComparer()),
                    state.NextInternalVariable
                )
            ];
        }
        else if (evaluation.Children.ElementAt(0) is Integer leftInt)
        {
            if (leftInt.Value != rightEvaluation)
            {
                return [];
            }

            return [new SolverState([], [], state.NextInternalVariable)];
        }

        return [];
    }
}

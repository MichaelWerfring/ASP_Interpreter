using asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.VariableRenaming;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals.ArithmeticEvaluation;

public class ArithmeticEvaluationGoal : IGoal
{
    private VariableSubstituter _substituter = new VariableSubstituter();
    private ArithmeticEvaluator _evaluator = new ArithmeticEvaluator();

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
        if(evaluation.Children.Count() != 2)
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

        if (evaluation.Children.ElementAt(1) is Variable leftVariable)
        {
            Dictionary<Variable, ISimpleTerm> substitution = new Dictionary<Variable, ISimpleTerm>(new VariableComparer())
            {
                {leftVariable, new Integer(rightEvaluation) }
            };

            var newGoals = state.CurrentGoals.Skip(1).Select((term) => _substituter.Substitute(term, substitution));
  
            return 
            [
                new SolverState
                (
                    newGoals, 
                    state.CurrentSubstitution.Union(substitution).ToDictionary(new VariableComparer()), 
                    state.NextInternalVariable
                )
            ];
        }
        else if(evaluation.Children.ElementAt(0) is Integer leftInt)
        {
            if (leftInt.Value != rightEvaluation)
            {
                return [];
            }

            return  [new SolverState([], [], state.NextInternalVariable)];
        }

        return [];
    }
}

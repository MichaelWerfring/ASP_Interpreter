using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals.ArithmeticEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalClasses.Goals.Comparison;

public class GreaterThanGoal : IGoal
{
    private ArithmeticEvaluator _evaluator = new ArithmeticEvaluator();

    public IEnumerable<SolverState> TrySatisfy(IDatabase database, SolverState state)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(state);

        if (state.CurrentGoals.Count() < 1) { throw new ArgumentException(nameof(state)); }

        Structure structure;
        try
        {
            structure = (Structure)state.CurrentGoals.First();
        }
        catch
        {
            throw new ArgumentException("Must be a structure.");
        }
        if (structure.Children.Count() != 2)
        {
            throw new ArgumentException(nameof(state));
        }

        var leftEvalMaybe = _evaluator.Evaluate(structure.Children.ElementAt(0));
        if (!leftEvalMaybe.HasValue) { return []; }

        var rightEvalMaybe = _evaluator.Evaluate(structure.Children.ElementAt(1));
        if (!rightEvalMaybe.HasValue) { return []; }

        var leftVal = leftEvalMaybe.GetValueOrThrow();
        var rightVal = rightEvalMaybe.GetValueOrThrow();

        if (!(leftVal > rightVal))
        {
            return [];
        }

        return [new SolverState(state.CurrentGoals.Skip(1), state.CurrentSubstitution, state.NextInternalVariable)];
    }
}

using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals.ArithmeticEvaluation;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals.Comparison;

public class SmallerThanOrEqualGoal : IGoal
{
    private ArithmeticEvaluator _evaluator;

    public SmallerThanOrEqualGoal(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable);

        _evaluator = new ArithmeticEvaluator(functorTable);
    }

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

        if (!(leftVal <= rightVal))
        {
            return [];
        }

        return [new SolverState(state.CurrentGoals.Skip(1), state.CurrentSubstitution, state.NextInternalVariable)];
    }
}

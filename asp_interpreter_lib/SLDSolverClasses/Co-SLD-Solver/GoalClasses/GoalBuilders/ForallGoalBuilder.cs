using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

public class ForallGoalBuilder : IGoalBuilder
{
    private readonly FunctorTableRecord _functors;

    public ForallGoalBuilder(FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));

        _functors = functors;
    }

    public ICoSLDGoal BuildGoal(CoSldSolverState currentState, IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(currentState, nameof(currentState));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        if (currentState.CurrentGoals.Count() < 1) { throw new ArgumentException(nameof(currentState)); }

        var goalTerm = currentState.CurrentGoals.First();

        if (goalTerm is not Structure forallStruct || forallStruct.Children.Count() != 2)
        { throw new ArgumentException("Must contain a structure term with two children.", nameof(currentState.CurrentGoals)); }

        if (forallStruct.Children.ElementAt(0) is not Variable var)
        {
            throw new ArgumentException("First child must be a variable.");
        }

        return new ForallGoal
            (database, var,forallStruct.Children.ElementAt(1),_functors,currentState.SolutionState);
    }
}

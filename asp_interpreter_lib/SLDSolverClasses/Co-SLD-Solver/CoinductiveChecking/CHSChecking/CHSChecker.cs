using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification.Standard;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;

public class CHSChecker
{
    private readonly ExactMatchChecker _checker = new(new StandardConstructiveUnificationAlgorithm(false));

    private readonly StandardConstructiveUnificationAlgorithm _algorithm = new(false);

    private readonly FunctorTableRecord _functors;

    private readonly GoalSolver _goalSolver;

    public CHSChecker(FunctorTableRecord record, GoalSolver solver)
    {
        ArgumentNullException.ThrowIfNull(record);
        ArgumentNullException.ThrowIfNull(solver);

        _functors = record;
        _goalSolver = solver;
    }

    public ICHSCheckingResult CheckCHS(Structure termToCheck, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(termToCheck, nameof(termToCheck));
        ArgumentNullException.ThrowIfNull(state);

        // construct negatedTerm
        ISimpleTerm negation = termToCheck.NegateTerm(_functors);


        if( state.CHS.Any( entry =>
        {
            return _checker.AreExactMatch(ConstructiveTargetBuilder.Build(negation, entry.Term, state.Mapping).GetValueOrThrow());
        }))
        {
            return new CHSDeterministicFailureResult();
        }

        if (state.CHS.Any(entry =>
        {
            return _checker.AreExactMatch(ConstructiveTargetBuilder.Build(termToCheck, entry.Term, state.Mapping).GetValueOrThrow()) && entry.HasSucceded;
        }))
        {
            return new CHSDeterministicSuccessResult();
        }


        // get all terms that unify with the negation of input entry, substitute them with mapping.
        IEnumerable<ISimpleTerm> unifyingTerms = state.CHS.Entries
            .Where(entry => _algorithm.Unify(ConstructiveTargetBuilder.Build(negation, entry.Term, state.Mapping).GetValueOrThrow()).HasValue)
            .Select(entry => entry.Term);

        // construct disunification goals
        IEnumerable<ISimpleTerm> disunificationGoals = unifyingTerms
            .Select(term => new Structure(_functors.Disunification, [termToCheck, term]));

        // from that, construct initial state for solver.
        var newSolverState = new CoSldSolverState
        (
            disunificationGoals,
            new SolutionState(state.Callstack, state.CHS, state.Mapping, state.NextInternalVariableIndex)
        );

        // return all the ways that all these disunifications can be solved.
        return new CHSConstrainmentResult
            (_goalSolver.SolveGoals(newSolverState).Select(x => x.ResultMapping));
    }
}

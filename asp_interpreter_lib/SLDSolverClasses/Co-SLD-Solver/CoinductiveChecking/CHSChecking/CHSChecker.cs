using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification.Standard;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;

public class CHSChecker
{
    private ConstructiveTargetBuilder _builder = new ConstructiveTargetBuilder();

    private ExactMatchChecker _checker = new ExactMatchChecker(new StandardConstructiveUnificationAlgorithm(false));

    private StandardConstructiveUnificationAlgorithm _algorithm = new(false);

    private FunctorTableRecord _functors;

    private GoalSolver _goalSolver;

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

        // check for exact matches
        foreach (var entry in state.Set.Entries)
        {
            if (_checker.AreExactMatch(_builder.Build(negation, entry.Term, state.Mapping)))
            {
                return new CHSDeterministicFailureResult();
            }

            if (_checker.AreExactMatch(_builder.Build(termToCheck, entry.Term, state.Mapping)) && entry.HasSucceded)
            {
                return new CHSDeterministicSuccessResult();
            }
        }

        // get all terms that unify with the negation of input entry, substitute them with mapping.
        var unifyingTerms = state.Set.Entries
            .Where(entry => _algorithm.Unify(_builder.Build(negation, entry.Term, state.Mapping)).HasValue)
            .Select(entry => entry.Term);

        // construct disunification goals
        IEnumerable<ISimpleTerm> disunificationGoals = unifyingTerms
            .Select(term => new Structure(_functors.Disunification, [termToCheck, term]));

        // from that, construct initial state for solver.
        var newSolverState = new CoSldSolverState
        (
            disunificationGoals,
            new SolutionState(state.Stack, state.Set, state.Mapping, state.NextInternalVariableIndex)
        );

        // return all the ways that all these disunifications can be solved.
        return new CHSConstrainmentResult
            (_goalSolver.SolveGoals(newSolverState).Select(x => x.ResultMapping));
    }
}

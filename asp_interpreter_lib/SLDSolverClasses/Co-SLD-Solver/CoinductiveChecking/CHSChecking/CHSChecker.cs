using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification.Standard;
using System.Collections.Immutable;

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
        foreach (var term in state.CurrentSet.Terms)
        {
            if (_checker.AreExactMatch(_builder.Build(negation, term, state.CurrentMapping)))
            {
                return new CHSDeterministicFailureResult();
            }

            if (_checker.AreExactMatch(_builder.Build(termToCheck, term, state.CurrentMapping)))
            {
                return new CHSDeterministicSuccessResult();
            }
        }

        // get all terms that unify with the negation of input term, substitute them with mapping.
        var unifyingTerms = state.CurrentSet.Terms
            .Where(term => _algorithm.Unify(_builder.Build(negation, term, state.CurrentMapping)).HasValue)
            .Select(state.CurrentMapping.ApplySubstitution);

        // construct disunification goals
        IEnumerable<ISimpleTerm> disunificationGoals = unifyingTerms
            .Select(term => new Structure(_functors.Disunification, [termToCheck, term]));

        // from that, construct initial state for solver.
        var newSolverState = new CoSldSolverState
        (
            disunificationGoals,
            new SolutionState(state.CurrentStack, state.CurrentSet, state.CurrentMapping, state.NextInternalVariableIndex)
        );

        // return all the ways that all these disunifications can be solved.
        return new CHSConstrainmentResult
            (_goalSolver.SolveGoals(newSolverState).Select(x => x.ResultMapping));
    }
}

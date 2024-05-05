using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.GoalMapping;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Unification.Constructive.Unification.Standard;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;

public class CHSChecker
{
    private ConstructiveTargetBuilder _builder = new();

    private VariableMappingSubstituter _substituter = new();

    private ExactMatchChecker _checker = new(new StandardConstructiveUnificationAlgorithm(false));

    private StandardConstructiveUnificationAlgorithm _algorithm = new(false);

    private FunctorTableRecord functors;

    private GoalSolver _goalSolver;

    public CHSChecker(FunctorTableRecord record, GoalSolver solver)
    {
        ArgumentNullException.ThrowIfNull(record);
        ArgumentNullException.ThrowIfNull(solver);

        functors = record;
        _goalSolver = solver;
    }

    public ICHSCheckingResult CheckCHS(Structure termToCheck, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(termToCheck, nameof(termToCheck));
        ArgumentNullException.ThrowIfNull(state);

        // update mapping, in case any variables in term have no value in state's variable mapping
        var updatedMapping = GetUpdatedMapping(termToCheck, state.CurrentMapping);

        // construct negatedTerm
        ISimpleTerm negation = NegateTerm(termToCheck);

        // check for exact matches
        //foreach (var term in state.CurrentSet.Terms)
        //{
        //    if (_checker.AreExactMatch(_builder.Build(negation, term, updatedMapping)))
        //    {
        //        return new CHSDeterministicFailureResult();
        //    }

        //    if (_checker.AreExactMatch(_builder.Build(termToCheck, term, updatedMapping)))
        //    {
        //        return new CHSDeterministicSuccessResult();
        //    }
        //}

        // get all terms that unify with the negation of input term
        var unifyingTerms = state.CurrentSet.Terms
            .Where(term => _algorithm.Unify(_builder.Build(negation, term, updatedMapping)).HasValue)
            .Select(term => _substituter.Substitute(term, updatedMapping));

        // construct disunification goals
        IEnumerable<ISimpleTerm> disunificationGoals = unifyingTerms
            .Select(term => new Structure(functors.Disunification, [termToCheck, term]));

        // from that, construct initial state for solver.
        var newSolverState = new CoSldSolverState
        (
            disunificationGoals.ToImmutableList(),
            new SolutionState(state.CurrentStack, state.CurrentSet, updatedMapping, state.NextInternalVariableIndex)
        );

        // return all the ways that all these disunifications can be solved
        return new CHSConstrainmentResult
            (_goalSolver.SolveGoals(newSolverState));
    }

    private ISimpleTerm NegateTerm(Structure termToCheck)
    {
        if (termToCheck.Functor == functors.NegationAsFailure && termToCheck.Children.Count == 1)
        {
            return termToCheck.Children.ElementAt(0);
        }
        else
        {
            return new Structure(functors.NegationAsFailure, [termToCheck]);
        }
    }

    private VariableMapping GetUpdatedMapping(Structure termToCheck, VariableMapping mapping)
    {
        var variablesInTerm = termToCheck.ExtractVariables();

        var newDict = mapping.Split().Item2
            .Select(x => (x.Key, (IVariableBinding)x.Value))
            .ToDictionary(new VariableComparer())
            .ToImmutableDictionary(new VariableComparer());
        foreach (var variable in variablesInTerm)
        {
            if (!mapping.Mapping.ContainsKey(variable))
            {
                newDict = newDict.SetItem(variable, new ProhibitedValuesBinding(ImmutableHashSet.Create<ISimpleTerm>(new SimpleTermEqualityComparer())));
            }
        }

        return new VariableMapping(newDict);
    }
}

// <copyright file="CHSChecker.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ConductiveChecking;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductiveChecking.CHSChecking.Results;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.ExactMatchChecking;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Unification.Constructive.Target.Builder;
using Asp_interpreter_lib.Unification.Constructive.Unification.Standard;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for checking a term against a coinductive hypothesis set.
/// </summary>
public class CHSChecker
{
    private readonly ExactMatchChecker checker;
    private readonly StandardConstructiveUnificationAlgorithm algorithm;
    private readonly FunctorTableRecord functorTable;
    private readonly GoalSolver goalSolver;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CHSChecker"/> class.
    /// </summary>
    /// <param name="checker">An exact match checker.</param>
    /// <param name="algo">A unification algorithm for general matches.</param>
    /// <param name="functorTable">A functor table for recognizing naf literals.</param>
    /// <param name="solver">A goalsover for disunifying the input term with all its negations in the chs.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..checker is null,
    /// ..algo is null,
    /// ..functorTable is null,
    /// ..logger is null.</exception>
    public CHSChecker(
        ExactMatchChecker checker,
        StandardConstructiveUnificationAlgorithm algo,
        FunctorTableRecord functorTable,
        GoalSolver solver,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(checker);
        ArgumentNullException.ThrowIfNull(algo);
        ArgumentNullException.ThrowIfNull(functorTable);
        ArgumentNullException.ThrowIfNull(solver);
        ArgumentNullException.ThrowIfNull(logger);

        this.checker = checker;
        this.algorithm = algo;
        this.functorTable = functorTable;
        this.goalSolver = solver;
        this.logger = logger;
    }

    /// <summary>
    /// Checks a term against a chs, contained in the input state.
    /// </summary>
    /// <param name="termToCheck">The term to check.</param>
    /// <param name="state">The state that contains chs and mapping.</param>
    /// <returns>A checking result.</returns>
    /// <exception cref="ArgumentNullException">THrown if..
    /// ..termToCheck is null,
    /// ..state is null.</exception>
    public ICHSCheckingResult CheckCHS(Structure termToCheck, SolutionState state)
    {
        ArgumentNullException.ThrowIfNull(termToCheck, nameof(termToCheck));
        ArgumentNullException.ThrowIfNull(state);

        this.logger.LogInfo($"Checking CHS for {termToCheck}");
        this.logger.LogTrace($"CHS is: {state.CHS}");
        this.logger.LogTrace($"Current mapping is: {state.Mapping}");

        // construct negatedTerm
        Structure negatedTerm = termToCheck.NegateTerm(this.functorTable);

        // check for exact matches for negatedTerm of term

        bool hasExactMatchForNegatedTerm;

        try
        {
            hasExactMatchForNegatedTerm = this.HasExactMatch(negatedTerm, state.CHS, state.Mapping, false);
        }
        catch
        {
            this.logger.LogError("Error during chs checking: term in chs contained variable that mapped to another term. " +
                "this is must likely due to a self-recursive structure.");
            return new CHSDeterministicFailureResult();
        }

        if (this.HasExactMatch(negatedTerm, state.CHS, state.Mapping, false))
        {
            return new CHSDeterministicFailureResult();
        }

        // check for exact matches for term
        if (this.HasExactMatch(termToCheck, state.CHS, state.Mapping, true))
        {
            return new CHSDeterministicSuccessResult();
        }

        // get all terms that unify with the negatedTerm of input entry.
        IEnumerable<Structure> termsUnifyingWithNegation = this.GetUnifyingTerms(negatedTerm, state.CHS, state.Mapping);

        // construct disunification goals
        IEnumerable<Structure> disunificationGoals = termsUnifyingWithNegation.AsParallel()
            .Select(term => new Structure(this.functorTable.Disunification, [termToCheck, term]));

        // from that, construct initial state for solver.
        var newSolverState = new CoSldSolverState(
            disunificationGoals,
            new SolutionState(state.Callstack, state.CHS, state.Mapping, state.NextInternalVariableIndex));

        // return all the ways that all these disunifications can be solved.
        return new CHSNoMatchOrConstrainmentResult(this.goalSolver.SolveGoals(newSolverState).Select(x => x.ResultMapping));
    }

    private bool HasExactMatch(ISimpleTerm term, CoinductiveHypothesisSet set, VariableMapping mapping, bool mustHaveSucceeded)
    {
        try
        {
            if (set.AsParallel().Any(entry =>
            {
                if (mustHaveSucceeded && !entry.HasSucceded)
                {
                    return false;
                }

                ConstructiveTarget target = ConstructiveTargetBuilder.Build(term, entry.Term, mapping).GetRightOrThrow();

                return this.checker.AreExactMatch(target);
            }))
            {
                return true;
            }
        }
        catch
        {
            throw;
        }

        return false;
    }

    private ParallelQuery<Structure> GetUnifyingTerms(Structure term, CoinductiveHypothesisSet set, VariableMapping mapping)
    {
        return set.AsParallel().Where(
            entry =>
            {
                var target = ConstructiveTargetBuilder.Build(term, entry.Term, mapping).GetRightOrThrow();

                return this.algorithm.Unify(target).HasValue;
            })
        .Select(entry => entry.Term);
    }
}
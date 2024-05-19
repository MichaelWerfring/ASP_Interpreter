using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.SLDSolverClasses.Basic.Events;
using asp_interpreter_lib.SLDSolverClasses.Basic.PostProcessing;
using asp_interpreter_lib.Unification.Basic.Interfaces;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDSolver;

public class SLDSolver
{
    private SubstitutionPostProcessor _postProcessor = new SubstitutionPostProcessor();

    private IUnificationAlgorithm _unificationAlgorithm;

    public SLDSolver(IUnificationAlgorithm unificationAlgorithm)
    {
        ArgumentNullException.ThrowIfNull(unificationAlgorithm);
        _unificationAlgorithm = unificationAlgorithm;
    }

    public void Solve(InternalAspProgram internalAspProgram)
    {
        ArgumentNullException.ThrowIfNull(internalAspProgram);

        Resolve(internalAspProgram.Query.ToList(), internalAspProgram.Statements.ToList(), new Dictionary<Variable, ISimpleTerm>(new VariableComparer()), 0);
    }

    public event EventHandler<SolutionFoundEventArgs>? SolutionFound;

    private void Resolve
    (
        IEnumerable<ISimpleTerm> goals,
        IEnumerable<IEnumerable<ISimpleTerm>> clauses,
        Dictionary<Variable, ISimpleTerm> mapping,
        int nextInternalID
    )
    {
        if (goals.Count() == 0)
        {
            SimplifyAndTrigger(mapping);
            return;
        }

        foreach (var clause in clauses)
        {
            ISimpleTerm currentGoal = goals.First();

            RenamingResult renamedClauseResult = clause.RenameClause(nextInternalID);

            ISimpleTerm renamedClauseHead = renamedClauseResult.RenamedClause.First();

            var substitutionMaybe = _unificationAlgorithm.Unify(currentGoal, renamedClauseHead);
            if (!substitutionMaybe.HasValue)
            {
                continue;
            }
            var substitution = substitutionMaybe.GetValueOrThrow();

            var newMapping = mapping.Union(substitution).ToDictionary(new VariableComparer());

            var nextGoals = renamedClauseResult.RenamedClause.Skip(1).Concat(goals.Skip(1));
            var substitutedGoals = nextGoals.Select((term) => term.Substitute(newMapping));

            Resolve(substitutedGoals, clauses, newMapping, renamedClauseResult.NextInternalIndex);
        }
    }

    private void SimplifyAndTrigger(Dictionary<Variable, ISimpleTerm> mapping)
    {
        var finalMapping = _postProcessor.Simplify(mapping);

        SolutionFound?.Invoke(this, new SolutionFoundEventArgs(finalMapping));
    }
}

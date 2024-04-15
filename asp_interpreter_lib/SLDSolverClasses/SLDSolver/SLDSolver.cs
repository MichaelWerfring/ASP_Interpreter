using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.Events;
using asp_interpreter_lib.SLDSolverClasses.SubstitutionPostProcessing;
using asp_interpreter_lib.SLDSolverClasses.VariableRenaming;
using asp_interpreter_lib.Unification.Interfaces;

namespace asp_interpreter_lib.SLDSolverClasses.StandardSolver;

public class SLDSolver
{
    private ClauseVariableRenamer _variableRenamer = new ClauseVariableRenamer();
    private VariableSubstituter _substituter = new VariableSubstituter();

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

            RenamingResult renamedClauseResult = _variableRenamer.RenameVariables(clause, nextInternalID);
            ISimpleTerm renamedClauseHead = renamedClauseResult.RenamedClause.First();

            var substitutionMaybe = _unificationAlgorithm.Unify(currentGoal, renamedClauseHead);
            if (!substitutionMaybe.HasValue)
            {
                continue;
            }
            var substitution = substitutionMaybe.GetValueOrThrow();

            var newMapping = mapping.Union(substitution).ToDictionary(new VariableComparer());

            var nextGoals = renamedClauseResult.RenamedClause.Skip(1).Concat(goals.Skip(1));
            var substitutedGoals = nextGoals.Select((term) => _substituter.Substitute(term, newMapping));

            Resolve(substitutedGoals, clauses, newMapping, renamedClauseResult.NextInternalIndex);
        }
    }

    private void SimplifyAndTrigger(Dictionary<Variable, ISimpleTerm> mapping)
    {
        var finalMapping = _postProcessor.Simplify(mapping);

        SolutionFound?.Invoke(this, new SolutionFoundEventArgs(finalMapping));
    }
}

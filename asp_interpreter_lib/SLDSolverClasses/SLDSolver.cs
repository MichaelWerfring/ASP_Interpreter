using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.SLDSolverClasses.VariableRenamer;
using asp_interpreter_lib.Unification.Interfaces;
using System.Diagnostics;

namespace asp_interpreter_lib.SLDSolverClasses;

public class SLDSolver
{
    private IUnificationAlgorithm _unificationAlgorithm;
    private VariableToInternalVariableRenamer _variableRenamer = new VariableToInternalVariableRenamer();
    private VariableSubstituter _substituter = new VariableSubstituter();

    public SLDSolver(IUnificationAlgorithm unificationAlgorithm)
    {
        ArgumentNullException.ThrowIfNull(unificationAlgorithm);
        _unificationAlgorithm = unificationAlgorithm;
    }

    public void Solve(InternalAspProgram internalAspProgram)
    {
        ArgumentNullException.ThrowIfNull(internalAspProgram);

        Resolve(internalAspProgram.Query.ToList(), internalAspProgram.Statements.ToList(), new Dictionary<Variable, IInternalTerm>(new VariableComparer()), 0);
    }


    private void Resolve
    (
        IEnumerable<IInternalTerm> goals,
        IEnumerable<IEnumerable<IInternalTerm>> clauses,
        Dictionary<Variable, IInternalTerm> mapping,
        int nextInternalID
    )
    {
        if (goals.Count() == 0)
        {
            Display(mapping);
            return;
        }

        foreach (var clause in clauses)
        {
            IInternalTerm currentGoal = goals.First();


            RenamingResult renamedClauseResult =_variableRenamer.RenameVariables(clause, nextInternalID);
            IInternalTerm renamedClauseHead = renamedClauseResult.RenamedClause.First();


            var substitutionMaybe = _unificationAlgorithm.Unify(new List<(IInternalTerm,IInternalTerm)>() { (currentGoal, renamedClauseHead)});

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

    private void Display(Dictionary<Variable, IInternalTerm> mapping)
    {
        Console.WriteLine("Found solution: ");
        Console.WriteLine("---------------------------------------------------------------------------------------------------");
        foreach ( var pair in mapping) 
        {
            Console.WriteLine($"{pair.Key} = {pair.Value}");
        }
        Console.WriteLine("---------------------------------------------------------------------------------------------------");
    }
}

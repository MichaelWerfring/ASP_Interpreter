using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.ClauseRenamer;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;

public class DatabaseUnifier
{
    private readonly IConstructiveUnificationAlgorithm _algorithm;

    private readonly IDatabase _database;

    private readonly ClauseVariableRenamer _renamer = new ();

    private readonly ConstructiveTargetBuilder _builder = new ();

    public DatabaseUnifier
    (
        IConstructiveUnificationAlgorithm algorithm,
        IDatabase database
    )
    {
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(database, nameof(database));

        _algorithm = algorithm;
        _database = database;
    }

    public IEnumerable<DBUnificationResult> GetDatabaseUnificationResults(Structure target, VariableMapping currentMapping, int nextInternal)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(currentMapping, nameof(currentMapping));

        foreach (var potentialUnification in _database.GetPotentialUnifications(target))
        {
            // rename potential goal clauses
            var renamingResult = _renamer.RenameVariables
                (potentialUnification, nextInternal);

            // unify
            var constructiveTarget = _builder.Build
                (target, renamingResult.RenamedClause.First(), currentMapping);
            VariableMapping unificationResult;
            try
            {
                unificationResult = _algorithm.Unify(constructiveTarget).GetValueOrThrow();
            }
            catch
            {
                continue;
            }

            // update with unification result
            var updatedMapping = currentMapping.Update(unificationResult).GetRightOrThrow();

            yield return new DBUnificationResult(renamingResult.RenamedClause.Skip(1), updatedMapping, renamingResult.NextInternalIndex);
        }
    }
}

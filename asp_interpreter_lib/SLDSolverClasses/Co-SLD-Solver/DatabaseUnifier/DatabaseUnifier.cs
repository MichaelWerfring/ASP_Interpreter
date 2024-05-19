using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Unification;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;

public class DatabaseUnifier
{
    private readonly IConstructiveUnificationAlgorithm _algorithm;

    private readonly IDatabase _database;

    private readonly ILogger _logger;

    public DatabaseUnifier(IConstructiveUnificationAlgorithm algorithm, IDatabase database, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(database, nameof(database));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _algorithm = algorithm;
        _database = database;
        _logger = logger;
    }

    public IEnumerable<DBUnificationResult> GetDatabaseUnificationResults
    (
        Structure target,
        VariableMapping currentMapping, 
        int nextInternal
    )
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(currentMapping, nameof(currentMapping));

        _logger.LogInfo($"Trying to find clauses that unify with {target}");
        _logger.LogDebug($"Mapping is {currentMapping}");

        foreach (var potentialUnification in _database.GetPotentialUnifications(target))
        {
            // rename potential goal clauses
            var renamingResult = potentialUnification.RenameClause(nextInternal);

            // unify
            var constructiveTarget = ConstructiveTargetBuilder.Build(target, renamingResult.RenamedClause.First(), currentMapping).GetRightOrThrow();

            var mappingMaybe = _algorithm.Unify(constructiveTarget);

            if (!mappingMaybe.HasValue)
            {
                continue;
            }

            VariableMapping unificationResult = mappingMaybe.GetValueOrThrow();

            _logger.LogInfo($"{target} unified with clause {potentialUnification.ToList().ListToString()}");

            _logger.LogDebug($"Unifying mapping is {unificationResult}");

            var updatedMapping = currentMapping.Update(unificationResult).GetValueOrThrow();
            _logger.LogTrace($"Updated mapping is {updatedMapping}");

            var flattenedMapping = updatedMapping.Flatten();
            _logger.LogTrace($"Flattened mapping is {flattenedMapping}");

            yield return new DBUnificationResult(renamingResult.RenamedClause, flattenedMapping, renamingResult.NextInternalIndex);
        }
    }
}

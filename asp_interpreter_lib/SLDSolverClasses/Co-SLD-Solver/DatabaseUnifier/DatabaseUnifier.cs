﻿using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.Unification.Constructive.Target.Builder;
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

        foreach (IEnumerable<Structure> potentialUnification in _database.GetPotentialUnifications(target))
        {
            _logger.LogInfo($"Trying to unify {target} with head of clause {potentialUnification.ToList().ListToString()}");

            // rename potential goal clauses
            RenamingResult renamingResult = TermFuncs.RenameClause(potentialUnification, nextInternal);

            _logger.LogTrace($"Renamed clause is: {renamingResult.RenamedClause}");

            // try build target
            var constructiveTargetEither = ConstructiveTargetBuilder.Build(target, renamingResult.RenamedClause.First(), currentMapping);

            ConstructiveTarget constructiveTarget = constructiveTargetEither.GetRightOrThrow();

            // try unify
            var unificationResultMaybe = _algorithm.Unify(constructiveTarget);
            if (!unificationResultMaybe.HasValue)
            {
                continue;
            }

            _logger.LogInfo($"Unified {target} with head of clause {potentialUnification.ToList().ListToString()}");

            VariableMapping unificationResult = unificationResultMaybe.GetValueOrThrow();
            _logger.LogTrace($"Unifying mapping is {unificationResult}");

            var updatedMapping = currentMapping.Update(unificationResult).GetValueOrThrow();
            _logger.LogTrace($"Updated mapping is {updatedMapping}");

            var flattenedMapping = updatedMapping.Flatten();
            _logger.LogTrace($"Flattened mapping is {flattenedMapping}");

            yield return new DBUnificationResult(renamingResult.RenamedClause, flattenedMapping, renamingResult.NextInternalIndex);
        }
    }
}

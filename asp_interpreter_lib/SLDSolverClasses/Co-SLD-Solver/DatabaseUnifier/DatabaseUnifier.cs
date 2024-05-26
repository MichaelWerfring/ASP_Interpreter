// <copyright file="DatabaseUnifier.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal;

using Asp_interpreter_lib.InternalProgramClasses.Database;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals.DBUnificationGoal.DBUnifier;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Unification.Constructive.Target.Builder;
using Asp_interpreter_lib.Unification.Constructive.Unification;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for unifying an input term with clauses in a database.
/// </summary>
public class DatabaseUnifier
{
    private readonly IConstructiveUnificationAlgorithm algorithm;

    private readonly IDatabase database;

    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseUnifier"/> class.
    /// </summary>
    /// <param name="algorithm">The unification algorithm to use.</param>
    /// <param name="database">The database to use.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">THrown if..
    /// ..<paramref name="algorithm"/> is null,
    /// ..<paramref name="database"/> is null,
    /// <paramref name="logger"/> is null.</exception>
    public DatabaseUnifier(IConstructiveUnificationAlgorithm algorithm, IDatabase database, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(algorithm, nameof(algorithm));
        ArgumentNullException.ThrowIfNull(database, nameof(database));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        this.algorithm = algorithm;
        this.database = database;
        this.logger = logger;
    }

    /// <summary>
    /// Attempts to unify the target with the head of a clause in the database.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="currentMapping">The current of the solver state.</param>
    /// <param name="nextInternal">The next internal variable.</param>
    /// <returns>An enumeration of results.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="target"/> is null,
    /// <paramref name="currentMapping"/> is null.</exception>
    public IEnumerable<DBUnificationResult> GetDatabaseUnificationResults
    (
        Structure target,
        VariableMapping currentMapping,
        int nextInternal
    )
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(currentMapping, nameof(currentMapping));

        foreach (IEnumerable<Structure> potentialUnification in this.database.GetPotentialUnifications(target))
        {
            this.logger.LogInfo($"Trying to unify {target} with head of clause {potentialUnification.ToList().ListToString()}");

            // rename potential goal clauses
            RenamingResult renamingResult = TermFuncs.RenameClause(potentialUnification, nextInternal);

            this.logger.LogTrace($"Renamed clause is: {renamingResult.RenamedClause}");

            // try build target
            var constructiveTargetEither = ConstructiveTargetBuilder.Build(target, renamingResult.RenamedClause.First(), currentMapping);

            ConstructiveTarget constructiveTarget = constructiveTargetEither.GetRightOrThrow();

            // try unify
            var unificationResultMaybe = this.algorithm.Unify(constructiveTarget);
            if (!unificationResultMaybe.HasValue)
            {
                continue;
            }

            this.logger.LogInfo($"Unified {target} with head of clause {potentialUnification.ToList().ListToString()}");

            VariableMapping unificationResult = unificationResultMaybe.GetValueOrThrow();
            this.logger.LogTrace($"Unifying mapping is {unificationResult}");

            var updatedMapping = currentMapping.Update(unificationResult).GetValueOrThrow();
            this.logger.LogTrace($"Updated mapping is {updatedMapping}");

            var flattenedMapping = updatedMapping.Flatten();
            this.logger.LogTrace($"Flattened mapping is {flattenedMapping}");

            yield return new DBUnificationResult(renamingResult.RenamedClause, flattenedMapping, renamingResult.NextInternalIndex);
        }
    }
}
// <copyright file="ClauseVariableRenamer.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

/// <summary>
/// A class for renaming a clause during the course of unification.
/// </summary>
public class ClauseVariableRenamer
{
    private readonly FunctorTableRecord functors;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClauseVariableRenamer"/> class.
    /// </summary>
    /// <param name="functors">The functorTable to use.</param>
    /// <exception cref="ArgumentNullException">Thrown if functorTable is null.</exception>
    public ClauseVariableRenamer(FunctorTableRecord functors)
    {
        ArgumentNullException.ThrowIfNull(functors, nameof(functors));

        this.functors = functors;
    }

    /// <summary>
    /// Renames the variables inside a clause.
    /// </summary>
    /// <param name="clause">The clause to rename.</param>
    /// <param name="currentInternalIndex">The next variable index to use.</param>
    /// <returns>A renaming result that contains the renamed clause and the next variable index.</returns>
    /// <exception cref="ArgumentNullException">Thrown if clause is null.</exception>
    public RenamingResult RenameVariables(IEnumerable<Structure> clause, int currentInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        var clauseVariables = clause
            .SelectMany(x => x.ExtractVariables())
            .Distinct(TermFuncs.GetSingletonVariableComparer());

        var varsToNewVarsKeyValuePairs = new List<KeyValuePair<Variable, ISimpleTerm>>();
        foreach (var var in clauseVariables)
        {
            varsToNewVarsKeyValuePairs.Add(new KeyValuePair<Variable, ISimpleTerm>(
                var, new Variable($"{this.functors.InternalVariable}{currentInternalIndex}")));

            currentInternalIndex += 1;
        }

        var dict = varsToNewVarsKeyValuePairs.
            ToDictionary(TermFuncs.GetSingletonVariableComparer());

        var substitutedClause = clause.Select((term) => term.Substitute(dict));

        return new RenamingResult(substitutedClause, currentInternalIndex);
    }
}
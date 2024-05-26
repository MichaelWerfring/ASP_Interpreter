// <copyright file="IDatabase.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.Database;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

/// <summary>
/// Represents an interface for a database.
/// </summary>
public interface IDatabase
{
    /// <summary>
    /// Enumerates potentially unifying clauses for the input term.
    /// </summary>
    /// <param name="term">A term to seek unfying clauses for.</param>
    /// <returns>An enumeration of clauses that might unify with the input term.</returns>
    public IEnumerable<IEnumerable<Structure>> GetPotentialUnifications(Structure term);
}

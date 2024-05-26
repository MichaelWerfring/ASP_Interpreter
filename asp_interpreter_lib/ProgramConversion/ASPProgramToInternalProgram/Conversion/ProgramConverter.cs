// <copyright file="ProgramConverter.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for conversion of asp programs.
/// </summary>
public class ProgramConverter : TypeBaseVisitor<ISimpleTerm>
{
    private readonly ILogger logger;
    private readonly FunctorTableRecord functorTable;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramConverter"/> class.
    /// </summary>
    /// <param name="functorTable">The functor table to use for conversion.</param>
    /// <param name="logger">A logger.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. functorTable is null.
    /// .. logger is null.</exception>
    public ProgramConverter(FunctorTableRecord functorTable, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(functorTable);
        ArgumentNullException.ThrowIfNull(logger);

        this.functorTable = functorTable;
        this.logger = logger;
    }

    /// <summary>
    /// Converts a query.
    /// </summary>
    /// <param name="query">The query to convert.</param>
    /// <returns>An enumerable of goals.</returns>
    /// <exception cref="ArgumentNullException">Thrown if query is null.</exception>
    public IEnumerable<Structure> ConvertQuery(Query query)
    {
        ArgumentNullException.ThrowIfNull(query);

        var converter = new GoalConverter(this.functorTable);

        return query.Goals.Select(x => converter.Convert(x).GetValueOrThrow()).ToList();
    }

    /// <summary>
    /// Converts a statement.
    /// </summary>
    /// <param name="statement">The statement to convert.</param>
    /// <returns>An cause in the form of an enumerable.</returns>
    /// <exception cref="ArgumentNullException">Thrown if statement is null.</exception>
    public IEnumerable<Structure> ConvertStatement(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        if (!statement.HasHead)
        {
            throw new ArgumentException("Must have a head!", nameof(statement));
        }

        var converter = new GoalConverter(this.functorTable);

        var list = new List<Structure>();

        var convertedHead = converter.Convert(statement.Head.GetValueOrThrow()).GetValueOrThrow();

        list.Add(convertedHead);

        foreach (var goal in statement.Body)
        {
            var convertedGoalMaybe = converter.Convert(goal);
            if (!convertedGoalMaybe.HasValue)
            {
                throw new Exception("Could not convert goal!");
            }

            list.Add(convertedGoalMaybe.GetValueOrThrow());
        }

        return list;
    }
}
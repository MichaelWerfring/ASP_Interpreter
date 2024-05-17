﻿using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

public class ProgramConverter : TypeBaseVisitor<ISimpleTerm>
{
    private readonly ILogger _logger;

    private readonly FunctorTableRecord _record;

    public ProgramConverter(FunctorTableRecord functorTable, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(functorTable);
        ArgumentNullException.ThrowIfNull(logger);

        _record = functorTable;
        _logger = logger;
    }

    public InternalAspProgram Convert(AspProgram prog)
    {
        _logger.LogTrace("Converting to internal program structure...");

        var clauses = prog.Statements.Where(rule => rule.Head.HasValue).Select(ConvertStatement).ToList();

        var queryMaybe = prog.Query;
        if (!queryMaybe.HasValue)
        {
            throw new ArgumentException("No query found!");
        }

        var goalConverterForQuery = new GoalConverter(_record);

        List<Structure> query = [];
        queryMaybe.GetValueOrThrow().Goals.ForEach(g => 
            query.Add(goalConverterForQuery.Convert(g).GetValueOrThrow("Goal cannot be converterd!")));
        

        return new InternalAspProgram(clauses, query);
    }

    public IEnumerable<Structure> ConvertStatement(Statement statement)
    {
        var converter = new GoalConverter(_record);

        var list = new List<Structure>();

        if (!statement.HasHead)
        {
            throw new ArgumentException("Must have a head!", nameof(statement));
        }
        var head = statement.Head.GetValueOrThrow();

        var convertedHeadMaybe = converter.Convert(head);
        if (!convertedHeadMaybe.HasValue)
        {
            throw new Exception("Could not convert head!");
        }

        list.Add(convertedHeadMaybe.GetValueOrThrow());



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
using asp_interpreter_lib.FunctorNaming;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

public class ProgramConverter : TypeBaseVisitor<ISimpleTerm>
{
    private readonly ILogger _logger;
    private readonly FunctorTableRecord _functors;

    public ProgramConverter(FunctorTableRecord functorTable, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(functorTable);
        ArgumentNullException.ThrowIfNull(logger);

        _functors = functorTable;
        _logger = logger;
    }

    public IEnumerable<Structure> ConvertQuery(Query query)
    {
        ArgumentNullException.ThrowIfNull(query);

        var converter = new GoalConverter(_functors);

        return query.Goals.Select(x => converter.Convert(x).GetValueOrThrow()).ToList();
    }

    public IEnumerable<Structure> ConvertStatement(Statement statement)
    {   
        ArgumentNullException.ThrowIfNull(statement);
        if (!statement.HasHead)
        {
            throw new ArgumentException("Must have a head!", nameof(statement));
        }


        var converter = new GoalConverter(_functors);

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
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

    private readonly GoalConverter _converter;

    public ProgramConverter(FunctorTableRecord functorTable, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(functorTable);
        ArgumentNullException.ThrowIfNull(logger);

        _converter = new GoalConverter(functorTable);
        _logger = logger;
    }

    public IEnumerable<Structure> ConvertQuery(Query query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return query.Goals.Select(x => _converter.Convert(x).GetValueOrThrow()).ToList();
    }

    public IEnumerable<Structure> ConvertStatement(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        if (!statement.HasHead)
        {
            throw new ArgumentException("Must have a head!", nameof(statement));
        }

        var list = new List<Structure>();

        var convertedHead = _converter.Convert(statement.Head.GetValueOrThrow()).GetValueOrThrow();

        list.Add(convertedHead);

        foreach (var goal in statement.Body)
        {
            var convertedGoalMaybe = _converter.Convert(goal);
            if (!convertedGoalMaybe.HasValue)
            {
                throw new Exception("Could not convert goal!");
            }

            list.Add(convertedGoalMaybe.GetValueOrThrow());
        }

        return list;
    }
}
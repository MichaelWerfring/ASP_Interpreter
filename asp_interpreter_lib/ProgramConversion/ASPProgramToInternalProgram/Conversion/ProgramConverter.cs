using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Mapping;

public class ProgramConverter : TypeBaseVisitor<ISimpleTerm>
{
    private GoalConverter _goalConverter;

    public ProgramConverter(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable);

        _goalConverter = new GoalConverter(functorTable);
    }

    public InternalAspProgram Convert(AspProgram prog)
    {
        var clauses = prog.Statements.Select(ConvertStatement).ToList();

        var queryMaybe = prog.Query.ClassicalLiteral.Accept(_goalConverter);
        if(!queryMaybe.HasValue)
        {
            throw new ArgumentException("Could not convert head!");
        }


        return new InternalAspProgram(clauses, [queryMaybe.GetValueOrThrow()]);
    }

    private IEnumerable<ISimpleTerm> ConvertStatement(Statement statement)
    {
        var list = new List<ISimpleTerm>();

        if(statement.HasHead)
        {
            var head = statement.Head.GetValueOrThrow();

            var convertedHeadMaybe = _goalConverter.Convert(head);
            if (!convertedHeadMaybe.HasValue)
            {
                throw new Exception("Could not convert head!");
            }

            list.Add(convertedHeadMaybe.GetValueOrThrow());
        }

        foreach (var goal in statement.Body)
        {
            var convertedGoalMaybe = _goalConverter.Convert(goal);
            if (!convertedGoalMaybe.HasValue)
            {
                throw new Exception("Could not convert goal!");
            }

            list.Add(convertedGoalMaybe.GetValueOrThrow());
        }

        return list;
    }
}
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Types;
using System.Text;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.ProgramToInternalProgramConversion;

public class StatementConverter
{
    private LiteralConverter _literalConverter = new LiteralConverter();
    private BinaryOperationConverter _binaryOperationConverter = new BinaryOperationConverter();
    
    private GoalToLiteralConverter _goalToLiteralConverter = new GoalToLiteralConverter();
    private GoalToBinaryOperationConverter _goalToBinaryOperationConverter = new GoalToBinaryOperationConverter();
    
    public IEnumerable<ISimpleTerm> Convert(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);

        ISimpleTerm convertedHead = _literalConverter.Convert(statement.Head.GetValueOrThrow());
        
        List<ISimpleTerm> convertedStatements = [];
        //_converter.Convert(goal);

        foreach (var goal in statement.Body)
        {
            goal.Accept(_goalToLiteralConverter).IfHasValue(v =>
            {
                convertedStatements.Add(_literalConverter.Convert(v));
            });
            goal.Accept(_goalToBinaryOperationConverter).IfHasValue(v =>
            {
                convertedStatements.Add(_binaryOperationConverter.Convert(v));
            });
            //_converter.Convert(goal);
        }

        return convertedStatements.Prepend(convertedHead);
    }
}

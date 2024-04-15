using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types;
using System.Text;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram;

public class StatementConverter
{
    private NafLiteralConverter _nafConverter = new NafLiteralConverter();
    private ClassicalLiteralConverter _classicalLiteralConverter = new ClassicalLiteralConverter();

    public IEnumerable<ISimpleTerm> Convert(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);

        ISimpleTerm convertedHead = ProcessHead(statement.Head);

        var convertedStatements = statement.Body.Literals.Select(_nafConverter.Convert);

        return convertedStatements.Prepend(convertedHead);
    }

    private ISimpleTerm ProcessHead(Head head)
    {
        if (head.Literal == null)
        {
            throw new ArgumentException("Must always have a head.");
        }

        return _classicalLiteralConverter.Convert(head.Literal);
    }
}

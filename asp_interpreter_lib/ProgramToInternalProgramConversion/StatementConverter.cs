using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.Types;
using System.Text;

namespace asp_interpreter_lib.ProgramToInternalProgramConversion;

public class StatementConverter
{
    private NafLiteralConverter _nafConverter = new NafLiteralConverter();
    private ClassicalLiteralConverter _classicalLiteralConverter = new ClassicalLiteralConverter();

    public IEnumerable<IInternalTerm> Convert(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);

        IInternalTerm convertedHead = ProcessHead(statement.Head);

        var convertedStatements = statement.Body.Literals.Select(_nafConverter.Convert);

        return convertedStatements.Prepend(convertedHead);
    }

    private IInternalTerm ProcessHead(Head head)
    {
        if (head.Literal == null)
        {
            throw new ArgumentException("Must always have a head.");
        }

        return _classicalLiteralConverter.Convert(head.Literal);
    }
}

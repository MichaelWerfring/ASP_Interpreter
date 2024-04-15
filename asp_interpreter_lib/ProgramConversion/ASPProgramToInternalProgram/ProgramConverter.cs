using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram;

public class ProgramConverter
{
    private QueryConverter _queryConverter = new QueryConverter();
    private StatementConverter _statementConverter = new StatementConverter();

    public InternalAspProgram Convert(AspProgram program)
    {
        ArgumentNullException.ThrowIfNull(program);

        var convertedStatements = program.Statements.Select(_statementConverter.Convert);
        var convertedQuery = _queryConverter.ProcessQuery(program.Query);

        return new InternalAspProgram(convertedStatements, new List<ISimpleTerm>() { convertedQuery });
    }
}

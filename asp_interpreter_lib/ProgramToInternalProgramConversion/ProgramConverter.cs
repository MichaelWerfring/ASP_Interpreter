using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.ProgramToInternalProgramConversion;

public class ProgramConverter
{
    private QueryConverter _queryConverter = new QueryConverter();
    private StatementConverter _statementConverter = new StatementConverter();

    public InternalAspProgram Preprocess(AspProgram program)
    {
        ArgumentNullException.ThrowIfNull(program);
       
        var convertedQuery = _queryConverter.ProcessQuery(program.Query);
        IEnumerable<IEnumerable<ISimpleTerm>> convertedStatements = program.Statements.Select(_statementConverter.Convert);

        return new InternalAspProgram(convertedStatements, convertedQuery);
    }
}

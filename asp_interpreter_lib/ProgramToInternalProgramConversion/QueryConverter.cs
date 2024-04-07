using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.ProgramToInternalProgramConversion;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.ProgramToInternalProgramConversion;

public class QueryConverter
{
    private ClassicalLiteralConverter _converter = new ClassicalLiteralConverter();

    public IEnumerable<IInternalTerm> ProcessQuery(Query query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return new List<IInternalTerm>() { _converter.Convert(query.ClassicalLiteral) };
    }
}

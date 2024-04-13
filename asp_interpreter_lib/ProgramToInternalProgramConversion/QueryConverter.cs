using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.ProgramToInternalProgramConversion;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.ProgramToInternalProgramConversion;

public class QueryConverter
{
    private readonly LiteralConverter _converter = new LiteralConverter();

    public IEnumerable<ISimpleTerm> ProcessQuery(Query query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return new List<ISimpleTerm>() { _converter.Convert(query.ClassicalLiteral) };
    }
}

using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram;

public class QueryConverter
{
    private ClassicalLiteralConverter _converter = new ClassicalLiteralConverter();

    public ISimpleTerm ProcessQuery(Query query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return _converter.Convert(query.ClassicalLiteral);
    }
}

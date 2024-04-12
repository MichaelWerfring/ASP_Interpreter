using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Types;
using System.Text;

namespace asp_interpreter_lib.ProgramToInternalProgramConversion;

public class ClassicalLiteralConverter
{
    private TermConverter _converter = new TermConverter();

    public ISimpleTerm Convert(ClassicalLiteral classicalLiteral)
    {
        ArgumentNullException.ThrowIfNull(classicalLiteral);

        var children = classicalLiteral.Terms.Select(_converter.Convert);

        var term = new Structure(classicalLiteral.Identifier, children);

        if(classicalLiteral.Negated)
        {
            term = new Structure("NEG", new List<ISimpleTerm>() { term });
        }

        return term;
    }
}

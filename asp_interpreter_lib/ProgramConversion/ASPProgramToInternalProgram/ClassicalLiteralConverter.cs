using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types;
using System.Text;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram;

public class ClassicalLiteralConverter
{
    private TermConverter _converter = new TermConverter();

    public ISimpleTerm Convert(ClassicalLiteral classicalLiteral)
    {
        ArgumentNullException.ThrowIfNull(classicalLiteral);

        var children = classicalLiteral.Terms.Select(_converter.Convert);

        ISimpleTerm term = new Structure(classicalLiteral.Identifier, children);
        if (classicalLiteral.Negated)
        {
            term = new ClassicalNegation(term);
        }

        return term;
    }
}

using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.ProgramToInternalProgramConversion;

public class NafLiteralConverter
{
    private ClassicalLiteralConverter _classicalLiteralConverter = new ClassicalLiteralConverter();
    private BuiltInAtomConverter _builtinConverter = new BuiltInAtomConverter();

    public ISimpleTerm Convert(NafLiteral nafLiteral)
    {
        ArgumentNullException.ThrowIfNull(nafLiteral);

        ISimpleTerm term;
        if(nafLiteral.IsClassicalLiteral)
        {
            term = _classicalLiteralConverter.Convert(nafLiteral.ClassicalLiteral);
        }
        else
        {
            term = _builtinConverter.Convert(nafLiteral.BinaryOperation);
        }

        if (nafLiteral.IsNafNegated)
        {
            term = new Structure("NOT", new List<ISimpleTerm>() { term });
        }
        return term;
    }
}


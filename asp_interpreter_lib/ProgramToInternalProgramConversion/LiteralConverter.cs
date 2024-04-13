using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.ProgramToInternalProgramConversion;

public class LiteralConverter
{
    private BinaryOperationConverter _builtinConverter = new BinaryOperationConverter();
    private TermConverter _converter = new TermConverter();
    
    public ISimpleTerm Convert(Literal literal)
    {
        ArgumentNullException.ThrowIfNull(literal);
        
        var children = literal.Terms.Select(_converter.Convert);

        var term = new Structure(literal.Identifier, children);

        if(literal.HasStrongNegation)
        {
            term = new Structure("NEG", new List<ISimpleTerm>() { term });
        }

        if (literal.HasNafNegation)
        {
            term = new Structure("NOT", new List<ISimpleTerm>() { term });
        }
        return term;
    }
}


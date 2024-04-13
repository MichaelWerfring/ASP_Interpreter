using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.TypeVisitors.Copy;

public class LiteralCopyVisitor(TypeBaseVisitor<ITerm> termCopyVisitor) : TypeBaseVisitor<Literal>
{
    public override IOption<Literal> Visit(Literal literal)
    {
        bool naf = literal.HasNafNegation;
        bool classical = literal.HasStrongNegation;
        string identifier = literal.Identifier.GetCopy();
        List<ITerm> terms = [];
        
        foreach (var term in literal.Terms)
        {
            terms.Add(term.Accept(termCopyVisitor).GetValueOrThrow(
                "The given term cannot be read!"));
        }
        
        return new Some<Literal>(new Literal(identifier, naf, classical, terms));
    }
}
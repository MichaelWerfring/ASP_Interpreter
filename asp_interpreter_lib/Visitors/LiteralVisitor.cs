using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Visitors;

public class LiteralVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<Literal>>
{
    private readonly IErrorLogger _errorLogger = errorLogger;

    public override IOption<Literal> VisitLiteral(ASPParser.LiteralContext context)
    {
        bool classicalNegation = context.MINUS() != null;
        bool nafNegation = context.NAF() != null;
        string id  = context.ID()?.GetText();

        if (string.IsNullOrEmpty(id))
        {
            _errorLogger.LogError("Cannot parse literal!", context);
            return new None<Literal>();
        }

        var terms = context.terms();

        if (terms ==null)
        {
            return new Some<Literal>(new Literal(id, nafNegation,classicalNegation , []));    
        }
        
        List<ITerm> parsedTerms = terms.Accept(new TermsVisitor(_errorLogger)).
            GetValueOrThrow($"Cannot parse terms of Literal {id}!").ToList();
        
        return new Some<Literal>(new Literal(id, nafNegation, classicalNegation, parsedTerms));
    }
}
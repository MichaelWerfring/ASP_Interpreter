using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Visitors;

public class LiteralVisitor(ILogger logger) : ASPParserBaseVisitor<IOption<Literal>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    public override IOption<Literal> VisitLiteral(ASPParser.LiteralContext context)
    {
        bool classicalNegation = context.MINUS() != null;
        bool nafNegation = context.NAF() != null;
        string id  = context.ID()?.GetText();

        if (string.IsNullOrEmpty(id))
        {
            _logger.LogError("Cannot parse literal!", context);
            return new None<Literal>();
        }

        var terms = context.terms();

        if (terms ==null)
        {
            return new Some<Literal>(new Literal(id, nafNegation,classicalNegation , []));    
        }
        
        List<ITerm> parsedTerms = [.. terms.Accept(new TermsVisitor(_logger)).GetValueOrThrow($"Cannot parse terms of Literal {id}!")];
        
        return new Some<Literal>(new Literal(id, nafNegation, classicalNegation, parsedTerms));
    }
}
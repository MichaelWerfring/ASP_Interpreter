using System.Net;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Visitors;

public class ClassicalLiteralVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<ClassicalLiteral>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override ClassicalLiteral VisitClassical_literal(ASPParser.Classical_literalContext context)
    {
        var negated = context.MINUS() !=  null;
        var id = context.ID().GetText() ?? 
                 throw new ArgumentException("The given literal has no id!");
        
        List<Term> terms = [];
        var termVisitor = new TermVisitor(_errorLogger);
        
        var childTerms = context.terms()?.children;

        if (childTerms == null)
        {
            return new ClassicalLiteral(id, negated, terms);
        }
        
        foreach (var t in childTerms)
        {
            Term term = t.Accept(termVisitor);
            if (term != null)
            {
                terms.Add(term);
            }
        }
        
        return new ClassicalLiteral(id, negated, terms);
    }
}
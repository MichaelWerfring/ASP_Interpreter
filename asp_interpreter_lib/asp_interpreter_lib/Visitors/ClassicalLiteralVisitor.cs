using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Visitors.TermVisitors;

namespace asp_interpreter_lib.Visitors;

public class ClassicalLiteralVisitor : ASPBaseVisitor<ClassicalLiteral>
{
    public override ClassicalLiteral VisitClassical_literal(ASPParser.Classical_literalContext context)
    {
        var negated = context.MINUS() !=  null;
        var id = context.ID().GetText();
        
        List<Term> terms = [];
        var termVisitor = new TermVisitor();
        
        var term = context.terms();
                
        terms.Add(term.Accept(termVisitor));

        var childTerms = term.terms();

        if (childTerms == null)
        {
            return new ClassicalLiteral(id, negated, terms); 
        }
        
        foreach (var t in term.terms().children)
        {
            Term te = t.Accept(termVisitor);
            terms.Add(te);
        }
        
        return new ClassicalLiteral(id, negated, terms);
    }
}
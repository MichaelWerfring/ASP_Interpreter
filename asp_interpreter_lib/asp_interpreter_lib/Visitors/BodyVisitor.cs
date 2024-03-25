using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Visitors;

public class BodyVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<Body>
{
    public override Body VisitBody(ASPParser.BodyContext context)
    {
        var children = context.children;
        List<NafLiteral> literals = [];
        var literalVisitor = new NafLiteralVisitor(errorLogger);
        
        foreach (var c in children)
        {
            var literal = c.Accept(literalVisitor);

            if (literal != null)
            {
                literals.Add(literal);   
            }
        }

        if (literals.Count == 0)
        {
            throw new  ArgumentException("A body must contain at least one literal");
        }
        
        return new Body(literals);
    }
}
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class StatementVisitor : ASPBaseVisitor<Statement>
{
    public override Statement VisitStatement(ASPParser.StatementContext context)
    {
        Head head = null;
        var headContext = context.head();
        
        if (headContext != null)
        {
            head = headContext.Accept(new HeadVisitor());
        }
        
        Body body = null;
        var bodyContext = context.body();

        if (bodyContext != null)
        {
            body = bodyContext.Accept(new BodyVisitor());
        }
        
        return new Statement(head, body);
    }
}
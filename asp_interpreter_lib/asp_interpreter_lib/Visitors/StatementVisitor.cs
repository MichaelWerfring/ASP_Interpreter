using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class StatementVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<Statement>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override Statement VisitStatement(ASPParser.StatementContext context)
    {
        Head head = null;
        var headContext = context.head();
        //Empty Statement per default
        //If Head or Body are found they will be added
        var statement = new Statement();
        
        if (headContext != null)
        {
            head = headContext.Accept(new HeadVisitor(_errorLogger));
            statement.AddHead(head);
        }
        
        Body body = null;
        var bodyContext = context.body();
        
        if (bodyContext != null)
        {
            body = bodyContext.Accept(new BodyVisitor(_errorLogger));
            statement.AddBody(body);
        }
        
        return statement;
    }
}
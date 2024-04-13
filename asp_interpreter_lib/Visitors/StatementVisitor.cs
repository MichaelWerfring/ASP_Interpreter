using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class StatementVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<Statement>>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    //public override IOption<InternalStatement> VisitStatement(ASPParser.StatementContext context)
    //{
    //    InternalHead head = null;
    //    var headContext = context.head();
    //    //Empty InternalStatement per default
    //    //If InternalHead or InternalBody are found they will be added
    //    var statement = new InternalStatement();
    //    
    //    if (headContext != null)
    //    {
    //        head = headContext.Accept(new HeadVisitor(_errorLogger));
    //        statement.AddHead(head);
    //    }
    //    
    //    InternalBody body = null;
    //    var bodyContext = context.body();
    //    
    //    if (bodyContext != null)
    //    {
    //        body = bodyContext.Accept(new BodyVisitor(_errorLogger));
    //        statement.AddBody(body);
    //    }
    //    
    //    return statement;
    //}
    
    public override IOption<Statement> VisitStatement(ASPParser.StatementContext context)
    {
        var statement = new Statement();
        
        List<Goal> body = [];
        LiteralVisitor literalVisitor = new(_errorLogger);
        
        var head = context.literal()?.Accept(literalVisitor);
        head?.IfHasValue((value) => statement.AddHead(value));
        
        BinaryOperationVisitor binaryOperationVisitor = new(_errorLogger);

        var goals = context.goal();

        if (goals == null)
        {
            //Just empty body
            return new Some<Statement>(statement);
        }
        
        foreach (var goal in goals)
        {
            var parsedGoal = goal.Accept(literalVisitor);

            if (parsedGoal.HasValue)
            {
                body.Add(parsedGoal.GetValueOrThrow());
                continue;
            }
            
            goal.Accept(binaryOperationVisitor).IfHasValue(v => body.Add(v));
        }

        if (goals.Length != body.Count)
        {
            _errorLogger.LogError("Not all goals could be parsed correctly", context);
            return new None<Statement>();
        }
        
        statement.AddBody(body);
        return new Some<Statement>(statement);
    }
}
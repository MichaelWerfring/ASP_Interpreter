﻿using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class StatementVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<Statement>>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    //public override IOption<Statement> VisitStatement(ASPParser.StatementContext context)
    //{
    //    Head head = null;
    //    var headContext = context.head();
    //    //Empty Statement per default
    //    //If Head or Body are found they will be added
    //    var statement = new Statement();
    //    
    //    if (headContext != null)
    //    {
    //        head = headContext.Accept(new HeadVisitor(_errorLogger));
    //        statement.AddHead(head);
    //    }
    //    
    //    Body body = null;
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
        //Empty Statement per default
        //If Head or Body are found they will be added
        var statement = new Statement();
        
        //In this case it is ok to ignore if head or body are null
        //because the statement can exist without both
        var head = context.head()?.Accept(new HeadVisitor(_errorLogger));
        var body = context.body()?.Accept(new BodyVisitor(_errorLogger));
        
        head?.IfHasValue((value) => statement.AddHead(value));
        body?.IfHasValue((value) => statement.AddBody(value));
        
        return new Some<Statement>(statement);
    }
}
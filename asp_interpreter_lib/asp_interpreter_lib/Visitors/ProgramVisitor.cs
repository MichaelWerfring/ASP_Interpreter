using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class ProgramVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<AspProgram>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override AspProgram VisitProgram(ASPParser.ProgramContext context)
    {
        List<Statement> statements = [];

        var statementVisitor = new StatementVisitor(_errorLogger);
        foreach (var s in context.statements().children)
        {
            statements.Add(s.Accept(statementVisitor));
        }
        
        var query = context.query().Accept(new QueryVisitor(_errorLogger));
        
        return new AspProgram(statements, query);
    }
}
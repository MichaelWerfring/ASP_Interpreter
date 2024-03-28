using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class ProgramVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<AspProgram>>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override IOption<AspProgram> VisitProgram(ASPParser.ProgramContext context)
    {
        List<Statement> statements = [];

        var statementVisitor = new StatementVisitor(_errorLogger);
        foreach (var statement in context.statements().children)
        {
            var result = statement.Accept(statementVisitor);
            
            if (!result.HasValue)
            {
                _errorLogger.LogError("Failed to program parse statement", context);
                return new None<AspProgram>();
            }
            
            statements.Add(result.GetValueOrThrow());
        }
        
        var query = context.query().Accept(new QueryVisitor(_errorLogger));

        if (!query.HasValue)
        {
            _errorLogger.LogError("Failed to parse query", context);
            return new None<AspProgram>();
        }
        
        return new Some<AspProgram>(new AspProgram(statements, query.GetValueOrThrow()));
    }
}
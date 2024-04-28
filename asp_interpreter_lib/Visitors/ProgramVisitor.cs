using asp_interpreter_lib.Types;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class ProgramVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<AspProgram>>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override IOption<AspProgram> VisitProgram(ASPParser.ProgramContext context)
    {
        //Try getting the query
        var query = context.query().Accept(new QueryVisitor(_errorLogger));

        if (!query.HasValue)
        {
            _errorLogger.LogError("Failed to parse query", context);
            return new None<AspProgram>();
        }
        
        //Parse the Statements
        var program = context.statements().children;

        if (program == null)
        {
            //Its still possible to have a query without any statements
            //The error message is just for clarification
            _errorLogger.LogError("Could not parse any statements!", context);
            return new Some<AspProgram>(new AspProgram([], query.GetValueOrThrow()));
        }
        
        List<Statement> statements = [];
        var statementVisitor = new StatementVisitor(_errorLogger);
        
        foreach (var statement in program)
        {
            var result = statement.Accept(statementVisitor);
            
            if (!result.HasValue)
            {
                _errorLogger.LogError("Failed to program parse statement", context);
                return new None<AspProgram>();
            }
            
            statements.Add(result.GetValueOrThrow());
        }
        
        return new Some<AspProgram>(new AspProgram(statements, query.GetValueOrThrow()));
    }
}
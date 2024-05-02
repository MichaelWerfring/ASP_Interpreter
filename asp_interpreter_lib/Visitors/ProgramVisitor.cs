using asp_interpreter_lib.Types;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class ProgramVisitor(ILogger logger) : ASPBaseVisitor<IOption<AspProgram>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    public override IOption<AspProgram> VisitProgram(ASPParser.ProgramContext context)
    {
        _logger.LogInfo("Parsing program...");

        //Try getting the query
        var query = context.query().Accept(new QueryVisitor(_logger));

        if (!query.HasValue)
        {
            _logger.LogError("Failed to parse query", context);
            return new None<AspProgram>();
        }
        
        //Parse the Statements
        var program = context.statements().children;

        if (program == null)
        {
            //Its still possible to have a query without any statements
            //The error message is just for clarification
            _logger.LogError("Could not parse any statements!", context);
            return new Some<AspProgram>(new AspProgram([], query));
        }
        
        List<Statement> statements = [];
        var statementVisitor = new StatementVisitor(_logger);
        
        foreach (var statement in program)
        {
            var result = statement.Accept(statementVisitor);
            
            if (!result.HasValue)
            {
                _logger.LogError("Failed to program parse statement", context);
                return new None<AspProgram>();
            }
            
            statements.Add(result.GetValueOrThrow());
        }

        return new Some<AspProgram>(new AspProgram(statements, query));
    }
}
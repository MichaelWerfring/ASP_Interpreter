using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Explaination;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class ProgramVisitor(ILogger logger) : ASPParserBaseVisitor<IOption<AspProgram>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    public override IOption<AspProgram> VisitProgram(ASPParser.ProgramContext context)
    {
        _logger.LogInfo("Parsing program...");

        //Try getting the query

        var query = context.query()?.Accept(new QueryVisitor(_logger));

        //Program does not need a query
        if (query == null)
        {
            _logger.LogInfo("Program has been specifed without a query.");
            query = new None<Query>();
        }
        
        //Parse the Statements
        var program = context.statements().children;

        if (program == null)
        {
            //Its still possible to have a query without any statements
            //The error message is just for clarification
            _logger.LogInfo("Program has been specifed without any statement.");
            return new Some<AspProgram>(new AspProgram([], query, []));
        }
        
        List<Explanation> explanations = [];
        List<Statement> statements = [];
        var statementVisitor = new StatementVisitor(_logger);
        var explanationVisitor = new ExplanationVisitor(_logger);
        foreach (var statement in program)
        {
            var result = statement.Accept(statementVisitor);
            
            if (result.HasValue)
            {
                statements.Add(result.GetValueOrThrow());    
            }
            
            var explanation = statement.Accept(explanationVisitor);
            if (explanation.HasValue)
            {
                explanations.Add(explanation.GetValueOrThrow());
            }
            
            _logger.LogError("Failed to program parse statement", context);
            return new None<AspProgram>();
        }

        return new Some<AspProgram>(new AspProgram(statements, query, explanations));
    }
}
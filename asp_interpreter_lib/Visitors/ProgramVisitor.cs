using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Visitors;

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
        
        Dictionary<(string,int), Explanation> explanations = [];
        List<Statement> statements = [];
        var statementVisitor = new StatementVisitor(_logger);
        var explanationVisitor = new ExplanationVisitor(_logger, new LiteralVisitor(_logger));
        foreach (var statement in program)
        {
            var result = statement.Accept(statementVisitor);
            
            if (result != null && result.HasValue)
            {
                statements.Add(result.GetValueOrThrow());
                continue;
            }
            
            statement.Accept(explanationVisitor)?.IfHasValue(
                v =>explanations.Add((v.Literal.Identifier, v.Literal.Terms.Count), v));
        }

        return new Some<AspProgram>(new AspProgram(statements, query, explanations));
    }
}
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class StatementVisitor(ILogger logger) : ASPBaseVisitor<IOption<Statement>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    public override IOption<Statement> VisitStatement(ASPParser.StatementContext context)
    {
        var statement = new Statement();
        
        List<Goal> body = [];
        LiteralVisitor literalVisitor = new(_logger);
        
        var head = context.literal()?.Accept(literalVisitor);
        head?.IfHasValue((value) => statement.AddHead(value));
        
        BinaryOperationVisitor binaryOperationVisitor = new(_logger);

        var goals = context.goal();

        if (goals == null)
        {
            //Just empty body
            return new Some<Statement>(statement);
        }

        var goalVisitor = new GoalVisitor(_logger);

        foreach (var goal in goals)
        {

            var parsedGoal = goal.Accept(goalVisitor);

            if (parsedGoal.HasValue)
            {
                body.Add(parsedGoal.GetValueOrThrow());
                continue;
            }
            
            goal.Accept(binaryOperationVisitor).IfHasValue(v => body.Add(v));
        }

        if (goals.Length != body.Count)
        {
            _logger.LogError("Not all goals could be parsed correctly", context);
            return new None<Statement>();
        }
        
        statement.AddBody(body);
        return new Some<Statement>(statement);
    }
}
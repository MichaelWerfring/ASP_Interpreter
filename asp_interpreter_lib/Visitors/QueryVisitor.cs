using System.Reflection.Metadata.Ecma335;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Visitors;

public class QueryVisitor(ILogger logger) : ASPBaseVisitor<IOption<Query>>
{
    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    private readonly GoalVisitor _goalVisitor = new(logger);
    
    public override IOption<Query> VisitQuery(ASPParser.QueryContext context)
    {
        //context.goal()
        //IOption<Literal> literal;
        //var literal = context.literal().Accept(new LiteralVisitor(_logger));
        List<Goal> query = [];
        for (int i = 0; i < context.children.Count; i++)
        {
            var goal = context.goal(i);

            goal?.Accept(_goalVisitor).
                IfHasValue(g => query.Add(g));
        }
        
        return new Some<Query>(new Query(query));
    }
}
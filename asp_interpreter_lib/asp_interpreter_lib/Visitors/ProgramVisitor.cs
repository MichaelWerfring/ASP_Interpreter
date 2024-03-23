using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class ProgramVisitor : ASPBaseVisitor<AspProgram>
{
    public override AspProgram VisitProgram(ASPParser.ProgramContext context)
    {
        List<Statement> statements = [];

        var statementVisitor = new StatementVisitor();
        foreach (var s in context.statements().children)
        {
            statements.Add(s.Accept(statementVisitor));
        }

        var queryVisitor = new QueryVisitor();
        var query = context.query().Accept(queryVisitor);
        
        return new AspProgram(statements, query);
    }
}
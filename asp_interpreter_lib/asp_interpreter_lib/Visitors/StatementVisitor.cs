using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class StatementVisitor : ASPBaseVisitor<Statement>
{
    public override Statement VisitStatement(ASPParser.StatementContext context)
    {
        return new Statement(null, null);
    }
}
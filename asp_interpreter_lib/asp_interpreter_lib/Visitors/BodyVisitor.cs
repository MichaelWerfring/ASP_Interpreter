using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class BodyVisitor : ASPBaseVisitor<Body>
{
    public override Body VisitBody(ASPParser.BodyContext context)
    {
        return base.VisitBody(context);
    }
}
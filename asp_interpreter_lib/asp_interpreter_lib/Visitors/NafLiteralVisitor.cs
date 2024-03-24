using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class NafLiteralVisitor : ASPBaseVisitor<NafLiteral>
{
    public override NafLiteral VisitNaf_literal(ASPParser.Naf_literalContext context)
    {
        //Here we need to check if the literal is invalid
        var classicalLiteral = context.classical_literal().Accept(new ClassicalLiteralVisitor());
        var naf = context.NAF() != null;
        
        return new NafLiteral(classicalLiteral, naf);
    }
}
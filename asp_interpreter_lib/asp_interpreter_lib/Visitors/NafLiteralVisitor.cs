using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class NafLiteralVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<NafLiteral>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override NafLiteral VisitNaf_literal(ASPParser.Naf_literalContext context)
    {
        //Still not initialized, its not clear if its a classical literal or a builtin atom
        NafLiteral literal = new NafLiteral(); 
        
        if (context.classical_literal() == null)
        {
            var atom = context.binary_operation().Accept(new BinaryOperationVisitor(_errorLogger));
            literal.AddBinaryOperation(atom);
        }

        if (context.binary_operation() == null)
        {
            var classicalLiteral = context.classical_literal().Accept(new ClassicalLiteralVisitor(_errorLogger));
            var negated = context.NAF() != null;
            literal.AddClassicalLiteral(classicalLiteral, negated);
        }
        

        
        return literal;
    }
}
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class NafLiteralVisitor : ASPBaseVisitor<NafLiteral>
{
    public override NafLiteral VisitNaf_literal(ASPParser.Naf_literalContext context)
    {
        //Still not initialized, its not clear if its a classical literal or a builtin atom
        NafLiteral literal = new NafLiteral(); 
        
        if (context.classical_literal() == null)
        {
            var atom = context.builtin_atom().Accept(new BuiltinAtomVisitor());
            literal.AddBuiltinAtom(atom);
        }

        if (context.builtin_atom() == null)
        {
            var classicalLiteral = context.classical_literal().Accept(new ClassicalLiteralVisitor());
            var negated = context.NAF() != null;
            literal.AddClassicalLiteral(classicalLiteral, negated);
        }

        return literal;
    }
}
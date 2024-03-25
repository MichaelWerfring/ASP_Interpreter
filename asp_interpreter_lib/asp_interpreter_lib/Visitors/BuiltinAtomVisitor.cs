using asp_interpreter_lib.Types;
using asp_interpreter_lib.Visitors.TermVisitors;

namespace asp_interpreter_lib.Visitors;

public class BuiltinAtomVisitor : ASPBaseVisitor<BuiltinAtom>
{
    public override BuiltinAtom VisitBuiltin_atom(ASPParser.Builtin_atomContext context)
    {
        var operation = context.binop().Accept(new BinaryOperationVisitor());
        var left = context.term(0).Accept(new TermVisitor());
        var right = context.term(1).Accept(new TermVisitor());
        return new BuiltinAtom(left,operation, right);
    }
}
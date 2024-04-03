using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Solving;

public class TypeNegator
{
    private readonly TypeBaseVisitor<BinaryOperator> _binaryOperationNegator;
    private readonly TypeBaseVisitor<ITerm> _termCopyVisitor;

    public TypeNegator(TypeBaseVisitor<BinaryOperator> binaryOperationNegator, TypeBaseVisitor<ITerm> termCopyVisitor)
    {
        _binaryOperationNegator = binaryOperationNegator;
        _termCopyVisitor = termCopyVisitor;
    }

    public NafLiteral NegateNaf(NafLiteral literal)
    {
        if (literal.IsBinaryOperation)
        {
            return new NafLiteral(NegateBinOp(literal.BinaryOperation));
        }
        
        return new NafLiteral(
            NegateClassical(literal.ClassicalLiteral),
            !literal.IsNafNegated);
    }

    private BinaryOperation NegateBinOp(BinaryOperation binop)
    {
        var newOperator = binop.BinaryOperator.Accept(_binaryOperationNegator);
        var newLeft = binop.Left.Accept(_termCopyVisitor);
        var newRight = binop.Right.Accept(_termCopyVisitor);
        
        if (newOperator.HasValue && newLeft.HasValue && newRight.HasValue)
        {
            return new BinaryOperation(newLeft.GetValueOrThrow(), newOperator.GetValueOrThrow(),
                newRight.GetValueOrThrow());
        }

        throw new InvalidOperationException("The given Term cannot be negated!");
    }

    private ClassicalLiteral NegateClassical(ClassicalLiteral literal)
    {
        List<ITerm> terms = [];
        literal.Terms.ForEach(t =>
        {
            var copy = t.Accept(_termCopyVisitor);
            terms.Add(copy.GetValueOrThrow("The given Term cannot be copied!"));
        });
        return new ClassicalLiteral(literal.Identifier.ToString(), literal.Negated, terms);
    }
}
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Visitors;

namespace asp_interpreter_lib.Types.TypeVisitors;

public class GoalNegator(
    TypeBaseVisitor<Literal> literalCopyVisitor) : TypeBaseVisitor<Goal>
{
    public override IOption<Goal> Visit(Literal literal)
    {
        Literal newLiteral =  literalCopyVisitor.Visit(literal).
            GetValueOrThrow("The literal cannot be copied!");
        
        newLiteral.HasNafNegation = !newLiteral.HasNafNegation;
        
        return new Some<Goal>(newLiteral);
    }

    public override IOption<Goal> Visit(BinaryOperation binop)
    {
        var newOperator = binop.BinaryOperator.Accept(new BinaryOperatorNegator());
        var newLeft = binop.Left.Accept(new TermCopyVisitor());
        var newRight = binop.Right.Accept(new TermCopyVisitor());
        
        if (newOperator.HasValue && newLeft.HasValue && newRight.HasValue)
        {
            return new Some<Goal>(new BinaryOperation(newLeft.GetValueOrThrow(), newOperator.GetValueOrThrow(),
                newRight.GetValueOrThrow()));
        }

        return new None<Goal>();
    }
}
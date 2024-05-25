using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types.TypeVisitors.Copy;

public class GoalCopyVisitor : TypeBaseVisitor<Goal>
{
    private readonly TermCopyVisitor _termCopyVisitor;

    public GoalCopyVisitor(TermCopyVisitor termCopyVisitor)
    {
        _termCopyVisitor = termCopyVisitor;
    }

    public override IOption<Goal> Visit(Forall goal)
    {
        var innerGoal = goal.Goal.Accept(this).GetValueOrThrow("The given goal cannot be copied!");
        var variable = new VariableTerm(goal.VariableTerm.Identifier.GetCopy());
        return new Some<Goal>(new Forall(variable, innerGoal));
    }

    public override IOption<Goal> Visit(Literal goal)
    {
        bool naf = goal.HasNafNegation;
        bool classical = goal.HasStrongNegation;
        string identifier = goal.Identifier.GetCopy();
        List<ITerm> terms = [];
        
        foreach (var term in goal.Terms)
        {
            terms.Add(term.Accept(_termCopyVisitor).GetValueOrThrow(
                "The given term cannot be read!"));
        }
        
        return new Some<Goal>(new Literal(identifier, naf, classical, terms));
    }

    public override IOption<Goal> Visit(BinaryOperation binOp)
    {
        var leftCopy = binOp.Left.Accept(_termCopyVisitor).GetValueOrThrow(
            "The given left term cannot be read!");
        var rightCopy = binOp.Right.Accept(_termCopyVisitor).GetValueOrThrow(
            "The given right term cannot be read!");

        return new Some<Goal>(new BinaryOperation(leftCopy, binOp.BinaryOperator, rightCopy));
    }
}
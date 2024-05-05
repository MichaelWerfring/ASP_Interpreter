﻿using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.TypeVisitors.Copy;

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

    public override IOption<Goal> Visit(BinaryOperation goal)
    {
        var leftCopy = goal.Left.Accept(_termCopyVisitor).GetValueOrThrow(
            "The given left term cannot be read!");
        var rightCopy = goal.Right.Accept(_termCopyVisitor).GetValueOrThrow(
            "The given right term cannot be read!");

        return new Some<Goal>(new BinaryOperation(leftCopy, goal.BinaryOperator, rightCopy));
    }
}
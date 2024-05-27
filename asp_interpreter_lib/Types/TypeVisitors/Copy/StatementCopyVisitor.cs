//-----------------------------------------------------------------------
// <copyright file="StatementCopyVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors.Copy;
using Asp_interpreter_lib.Util.ErrorHandling;

public class StatementCopyVisitor : TypeBaseVisitor<Statement>
{
    // private readonly BinaryOperationVisitor _binaryOperationVisitor;
    private readonly LiteralCopyVisitor literalCopyVisitor;

    private readonly GoalCopyVisitor goalCopyVisitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="StatementCopyVisitor"/> class.
    /// </summary>
    public StatementCopyVisitor()
    {
        var termCopyVisitor = new TermCopyVisitor();
        this.literalCopyVisitor = new LiteralCopyVisitor(termCopyVisitor);
        this.goalCopyVisitor = new GoalCopyVisitor(termCopyVisitor);
    }

    /// <inheritdoc/>
    public override IOption<Statement> Visit(Statement statement)
    {
        Statement copy = new();

        if (statement.HasHead)
        {
            statement.Head.GetValueOrThrow().Accept(this.literalCopyVisitor).IfHasValue(
                copy.AddHead);
        }

        List<Goal> body =[];
        foreach (var goal in statement.Body)
        {
            goal.Accept(this.goalCopyVisitor).IfHasValue(body.Add);
        }

        if (body.Count != statement.Body.Count)
        {
            throw new InvalidOperationException("The body of the statement could not be copied!");
        }

        copy.AddBody(body);

        return new Some<Statement>(copy);
    }
}
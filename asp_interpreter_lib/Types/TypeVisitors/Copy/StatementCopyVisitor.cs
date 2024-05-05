using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.TypeVisitors.Copy;

public class StatementCopyVisitor : TypeBaseVisitor<Statement>
{
    //private readonly BinaryOperationVisitor _binaryOperationVisitor;
    private readonly LiteralCopyVisitor _literalCopyVisitor;

    private readonly GoalCopyVisitor _goalCopyVisitor;

    public StatementCopyVisitor()
    {
        var termCopyVisitor = new TermCopyVisitor();
        _literalCopyVisitor = new LiteralCopyVisitor(termCopyVisitor);
        _goalCopyVisitor = new GoalCopyVisitor(termCopyVisitor);
    }
    
    public override IOption<Statement> Visit(Statement statement)
    {
        Statement copy = new();

        if (statement.HasHead)
        {
            statement.Head.GetValueOrThrow().Accept(_literalCopyVisitor).IfHasValue(
                head => copy.AddHead(head));
        }
        
        List<Goal> body = [];
        foreach (var goal in statement.Body)
        {
            goal.Accept(_goalCopyVisitor).IfHasValue(g => body.Add(g));
        }

        if (body.Count != statement.Body.Count)
        {
            throw new InvalidOperationException("The body of the statement could not be copied!");
        }
        
        copy.AddBody(body);
        
        return new Some<Statement>(copy);
    }
}